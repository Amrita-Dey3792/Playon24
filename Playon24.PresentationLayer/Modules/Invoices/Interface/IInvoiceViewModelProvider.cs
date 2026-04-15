using Playon24.PresentationLayer.Modules.Invoices.ViewModels;

namespace Playon24.PresentationLayer.Modules.Invoices.Interface;

public interface IInvoiceViewModelProvider
{
    Task<InvoiceSellViewModel> GetSellViewModelAsync();
    Task<int> CreateSaleAsync(InvoiceSellViewModel model);
    Task<InvoiceListPageViewModel> GetListPageAsync(int? customerId, string? search);
}
