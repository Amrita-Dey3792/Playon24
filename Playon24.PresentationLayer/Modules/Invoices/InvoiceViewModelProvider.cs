using Playon24.BusinessLayer.Modules.Customers.Interface;
using Playon24.BusinessLayer.Modules.Invoices.Interface;
using Playon24.BusinessLayer.Modules.Products.Interface;
using Playon24.PresentationLayer.Modules.Invoices.Interface;
using Playon24.PresentationLayer.Modules.Invoices.ViewModels;

namespace Playon24.PresentationLayer.Modules.Invoices;

public class InvoiceViewModelProvider : IInvoiceViewModelProvider
{
    private readonly IInvoiceService _invoiceService;
    private readonly ICustomerService _customerService;
    private readonly IProductService _productService;

    public InvoiceViewModelProvider(
        IInvoiceService invoiceService,
        ICustomerService customerService,
        IProductService productService)
    {
        _invoiceService = invoiceService;
        _customerService = customerService;
        _productService = productService;
    }

    public async Task<InvoiceSellViewModel> GetSellViewModelAsync()
    {
        var customers = await _customerService.GetAllAsync();
        var products = await _productService.GetAllAsync();

        var vm = new InvoiceSellViewModel
        {
            CustomerOptions = customers
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .Select(c => new CustomerSelectOption
                {
                    Id = c.Id,
                    DisplayName = $"{c.FirstName} {c.LastName} ({c.Email})"
                })
                .ToList(),
            ProductOptions = products
                .Where(p => p.IsActive)
                .OrderBy(p => p.ProductName)
                .Select(p => new ProductSelectOption
                {
                    Id = p.Id,
                    DisplayName = $"{p.ProductName} — {p.UnitPrice:0.00} (stock {p.StockQuantity})",
                    UnitPrice = p.UnitPrice
                })
                .ToList()
        };

        for (var i = 0; i < 5; i++)
            vm.Lines.Add(new InvoiceLineInputViewModel());

        return vm;
    }

    public async Task<int> CreateSaleAsync(InvoiceSellViewModel model)
    {
        var lines = model.Lines
            .Where(l => l.ProductId > 0 && l.Quantity > 0)
            .Select(l => new InvoiceLineRequest(l.ProductId, l.Quantity, l.DiscountPercent))
            .ToList();

        var invoice = await _invoiceService.CreateSaleAsync(
            model.CustomerId,
            model.DiscountAmount,
            model.TaxAmount,
            model.PaymentStatus,
            lines);

        return invoice.InvoiceID;
    }

    public async Task<InvoiceListPageViewModel> GetListPageAsync(int? customerId, string? search)
    {
        var customers = await _customerService.GetAllAsync();
        var invoices = await _invoiceService.GetListAsync(customerId, search);

        return new InvoiceListPageViewModel
        {
            FilterCustomerId = customerId,
            Search = search,
            CustomerOptions = customers
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .Select(c => new CustomerSelectOption
                {
                    Id = c.Id,
                    DisplayName = $"{c.FirstName} {c.LastName}"
                })
                .ToList(),
            Invoices = invoices.Select(i => new InvoiceListRowViewModel
            {
                InvoiceId = i.InvoiceID,
                InvoiceNumber = i.InvoiceNumber,
                CustomerName = $"{i.Customer.FirstName} {i.Customer.LastName}".Trim(),
                CreatedAt = i.CreatedAt,
                GrandTotal = i.GrandTotal,
                PaymentStatus = i.PaymentStatus
            }).ToList()
        };
    }
}
