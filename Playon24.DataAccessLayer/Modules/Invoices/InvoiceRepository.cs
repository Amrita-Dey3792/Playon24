using Microsoft.EntityFrameworkCore;
using Playon24.DataAccessLayer.Data;
using Playon24.DataAccessLayer.Modules.Invoices.Interfaces;
using Playon24.Domain.Entities;

namespace Playon24.DataAccessLayer.Modules.Invoices;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly Payon24DbContext _db;

    public InvoiceRepository(Payon24DbContext db)
    {
        _db = db;
    }

    public async Task<Invoice?> GetByIdWithDetailsAsync(int invoiceId)
    {
        return await _db.Invoices
            .AsNoTracking()
            .Include(i => i.Customer)
            .Include(i => i.InvoiceDetails)
            .ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(i => i.InvoiceID == invoiceId);
    }

    public async Task<IReadOnlyList<Invoice>> GetListAsync(int? customerId, string? search)
    {
        var q = _db.Invoices
            .AsNoTracking()
            .Include(i => i.Customer)
            .OrderByDescending(i => i.InvoiceID)
            .AsQueryable();

        if (customerId is > 0)
            q = q.Where(i => i.CustomerID == customerId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            q = q.Where(i =>
                i.InvoiceNumber.Contains(s)
                || i.Customer.FirstName.Contains(s)
                || i.Customer.LastName.Contains(s)
                || i.Customer.Email.Contains(s));
        }

        return await q.ToListAsync();
    }

    public async Task<Invoice> CreateSaleAsync(Invoice invoice, IReadOnlyList<InvoiceDetail> details)
    {
        await using var tx = await _db.Database.BeginTransactionAsync();
        try
        {
            foreach (var line in details)
            {
                var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == line.ProductID);
                if (product == null)
                    throw new InvalidOperationException($"Product {line.ProductID} not found.");

                if (!product.IsActive)
                    throw new InvalidOperationException($"Product '{product.ProductName}' is not active.");

                if (product.StockQuantity < line.Quantity)
                    throw new InvalidOperationException(
                        $"Insufficient stock for '{product.ProductName}'. Available: {product.StockQuantity}.");

                product.StockQuantity -= line.Quantity;
                product.UpdatedAt = DateTime.UtcNow;
            }

            var count = await _db.Invoices.CountAsync();
            invoice.InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMdd}-{(count + 1):D4}";

            await _db.Invoices.AddAsync(invoice);
            await _db.SaveChangesAsync();

            foreach (var d in details)
            {
                d.InvoiceID = invoice.InvoiceID;
                d.CreatedAt = invoice.CreatedAt;
                d.UpdatedAt = invoice.UpdatedAt;
            }

            await _db.InvoiceDetails.AddRangeAsync(details);
            await _db.SaveChangesAsync();

            await tx.CommitAsync();
            return invoice;
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }
}
