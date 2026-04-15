namespace Playon24.BusinessLayer.Modules.Dashboard.Interface;

public interface IDashboardService
{
    Task<DashboardSummary> GetSummaryAsync(int recentInvoiceCount = 8);
}
