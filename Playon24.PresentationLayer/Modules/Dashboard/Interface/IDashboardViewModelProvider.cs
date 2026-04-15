using Playon24.PresentationLayer.Modules.Dashboard.ViewModels;

namespace Playon24.PresentationLayer.Modules.Dashboard.Interface;

public interface IDashboardViewModelProvider
{
    Task<DashboardSummaryViewModel> GetSummaryAsync();
}
