using Playon24.BusinessLayer.Exceptions;
using Playon24.BusinessLayer.Modules.Invoices.Interface;
using Playon24.DataAccessLayer.Modules.Customers.Interfaces;
using Playon24.DataAccessLayer.Modules.Invoices.Interfaces;
using Playon24.DataAccessLayer.Modules.Products.Interfaces;
using Playon24.Domain.Entities;

namespace Playon24.BusinessLayer.Modules.Invoices;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoices;
    private readonly ICustomerRepository _customers;
    private readonly IProductRepository _products;

    public InvoiceService(
        IInvoiceRepository invoices,
        ICustomerRepository customers,
        IProductRepository products)
    {
        _invoices = invoices;
        _customers = customers;
        _products = products;
    }

    public Task<Invoice?> GetByIdWithDetailsAsync(int invoiceId)
    {
        return _invoices.GetByIdWithDetailsAsync(invoiceId);
    }

    public Task<IReadOnlyList<Invoice>> GetListAsync(int? customerId, string? search)
    {
        return _invoices.GetListAsync(customerId, search);
    }

    public async Task<Invoice> CreateSaleAsync(
        int customerId,
        decimal discountAmount,
        decimal taxAmount,
        string paymentStatus,
        IReadOnlyList<InvoiceLineRequest> lines)
    {
        if (customerId <= 0)
            throw new InvalidUserInputException("Select a customer.");

        if (lines == null || lines.Count == 0)
            throw new InvalidUserInputException("Add at least one product line.");

        if (discountAmount < 0 || taxAmount < 0)
            throw new InvalidUserInputException("Discount and tax cannot be negative.");

        paymentStatus = string.IsNullOrWhiteSpace(paymentStatus) ? "Paid" : paymentStatus.Trim();
        if (paymentStatus.Length > 20)
            paymentStatus = paymentStatus[..20];

        var customer = await _customers.GetByIdAsync(customerId);
        if (customer == null)
            throw new InvalidUserInputException("Customer not found.");

        var now = DateTime.UtcNow;
        var detailEntities = new List<InvoiceDetail>();
        decimal subTotal = 0;

        foreach (var line in lines)
        {
            if (line.ProductId <= 0 || line.Quantity <= 0)
                throw new InvalidUserInputException("Each line needs a product and a positive quantity.");

            if (line.DiscountPercent < 0 || line.DiscountPercent > 100)
                throw new InvalidUserInputException("Line discount must be between 0 and 100.");

            var product = await _products.GetByIdAsync(line.ProductId);
            if (product == null)
                throw new InvalidUserInputException($"Product id {line.ProductId} was not found.");

            var unitPrice = product.UnitPrice;
            var lineTotal = Math.Round(line.Quantity * unitPrice * (1 - line.DiscountPercent / 100m), 2);
            subTotal += lineTotal;

            detailEntities.Add(new InvoiceDetail
            {
                ProductID = line.ProductId,
                Quantity = line.Quantity,
                UnitPrice = unitPrice,
                DiscountPercent = line.DiscountPercent,
                LineTotal = lineTotal,
                CreatedAt = now,
                UpdatedAt = now
            });
        }

        var grandTotal = Math.Round(subTotal - discountAmount + taxAmount, 2);
        if (grandTotal < 0)
            throw new InvalidUserInputException("Grand total cannot be negative. Adjust discount or tax.");

        var invoice = new Invoice
        {
            CustomerID = customerId,
            SubTotal = subTotal,
            DiscountAmount = discountAmount,
            TaxAmount = taxAmount,
            GrandTotal = grandTotal,
            PaymentStatus = paymentStatus,
            CreatedAt = now,
            UpdatedAt = now
        };

        try
        {
            return await _invoices.CreateSaleAsync(invoice, detailEntities);
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidUserInputException(ex.Message);
        }
    }
}
