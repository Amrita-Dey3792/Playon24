using Playon24.BusinessLayer.Modules.Dashboard.Interface;
using Playon24.PresentationLayer.Modules.Dashboard.Interface;
using Playon24.PresentationLayer.Modules.Dashboard.ViewModels;

namespace Playon24.PresentationLayer.Modules.Dashboard;

public class DashboardViewModelProvider : IDashboardViewModelProvider
{
    private readonly IDashboardService _dashboardService;

    public DashboardViewModelProvider(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<DashboardSummaryViewModel> GetSummaryAsync()
    {
        var data = await _dashboardService.GetSummaryAsync(8);
        return new DashboardSummaryViewModel
        {
            TotalProducts = data.TotalProducts,
            ActiveProducts = data.ActiveProducts,
            LowStockProducts = data.LowStockProducts,
            TotalCustomers = data.TotalCustomers,
            TotalInvoices = data.TotalInvoices,
            InvoiceRevenueTotal = data.InvoiceRevenueTotal,
            LowStockThreshold = 10,
            RecentInvoices = data.RecentInvoices.Select(r => new DashboardRecentInvoiceViewModel
            {
                InvoiceId = r.InvoiceId,
                InvoiceNumber = r.InvoiceNumber,
                CustomerName = r.CustomerName.Trim(),
                CreatedAt = r.CreatedAt,
                GrandTotal = r.GrandTotal,
                PaymentStatus = r.PaymentStatus
            }).ToList()
        };
    }
}
