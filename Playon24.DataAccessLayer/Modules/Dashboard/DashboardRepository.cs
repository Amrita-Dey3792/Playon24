using Microsoft.EntityFrameworkCore;
using Playon24.DataAccessLayer.Data;
using Playon24.DataAccessLayer.Modules.Dashboard.Interfaces;

namespace Playon24.DataAccessLayer.Modules.Dashboard;

public class DashboardRepository : IDashboardRepository
{
    private const int LowStockThreshold = 10;

    private readonly Payon24DbContext _db;

    public DashboardRepository(Payon24DbContext db)
    {
        _db = db;
    }

    public async Task<DashboardSummaryData> GetSummaryAsync(int recentInvoiceCount = 8)
    {
        var totalProducts = await _db.Products.CountAsync();
        var activeProducts = await _db.Products.CountAsync(p => p.IsActive);
        var lowStock = await _db.Products.CountAsync(p => p.IsActive && p.StockQuantity <= LowStockThreshold);
        var customers = await _db.Customers.CountAsync();
        var invoices = await _db.Invoices.CountAsync();
        var revenue = await _db.Invoices.SumAsync(i => (decimal?)i.GrandTotal) ?? 0m;

        var recentEntities = await _db.Invoices
            .AsNoTracking()
            .Include(i => i.Customer)
            .OrderByDescending(i => i.InvoiceID)
            .Take(recentInvoiceCount)
            .ToListAsync();

        var recent = recentEntities.Select(i => new RecentInvoiceRowData
        {
            InvoiceId = i.InvoiceID,
            InvoiceNumber = i.InvoiceNumber,
            CustomerName = $"{i.Customer.FirstName} {i.Customer.LastName}".Trim(),
            CreatedAt = i.CreatedAt,
            GrandTotal = i.GrandTotal,
            PaymentStatus = i.PaymentStatus
        }).ToList();

        return new DashboardSummaryData
        {
            TotalProducts = totalProducts,
            ActiveProducts = activeProducts,
            LowStockProducts = lowStock,
            TotalCustomers = customers,
            TotalInvoices = invoices,
            InvoiceRevenueTotal = revenue,
            RecentInvoices = recent
        };
    }
}
