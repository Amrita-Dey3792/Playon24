using Playon24.DataAccessLayer.Modules.Dashboard;

namespace Playon24.DataAccessLayer.Modules.Dashboard.Interfaces;

public interface IDashboardRepository
{
    Task<DashboardSummaryData> GetSummaryAsync(int recentInvoiceCount = 8);
}
