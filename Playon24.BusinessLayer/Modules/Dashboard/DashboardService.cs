using Playon24.BusinessLayer.Modules.Dashboard.Interface;
using Playon24.DataAccessLayer.Modules.Dashboard.Interfaces;

namespace Playon24.BusinessLayer.Modules.Dashboard;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _repository;

    public DashboardService(IDashboardRepository repository)
    {
        _repository = repository;
    }

    public async Task<DashboardSummary> GetSummaryAsync(int recentInvoiceCount = 8)
    {
        var data = await _repository.GetSummaryAsync(recentInvoiceCount);
        return new DashboardSummary
        {
            TotalProducts = data.TotalProducts,
            ActiveProducts = data.ActiveProducts,
            LowStockProducts = data.LowStockProducts,
            TotalCustomers = data.TotalCustomers,
            TotalInvoices = data.TotalInvoices,
            InvoiceRevenueTotal = data.InvoiceRevenueTotal,
            RecentInvoices = data.RecentInvoices.Select(r => new DashboardRecentInvoice
            {
                InvoiceId = r.InvoiceId,
                InvoiceNumber = r.InvoiceNumber,
                CustomerName = r.CustomerName,
                CreatedAt = r.CreatedAt,
                GrandTotal = r.GrandTotal,
                PaymentStatus = r.PaymentStatus
            }).ToList()
        };
    }
}
