namespace Playon24.PresentationLayer.Modules.Invoices.ViewModels;

public class InvoiceListPageViewModel
{
    public int? FilterCustomerId { get; set; }
    public string? Search { get; set; }
    public List<CustomerSelectOption> CustomerOptions { get; set; } = new();
    public List<InvoiceListRowViewModel> Invoices { get; set; } = new();
}

public class InvoiceListRowViewModel
{
    public int InvoiceId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public decimal GrandTotal { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
}
