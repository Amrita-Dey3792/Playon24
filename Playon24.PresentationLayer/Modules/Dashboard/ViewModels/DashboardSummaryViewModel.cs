namespace Playon24.PresentationLayer.Modules.Dashboard.ViewModels;

public class DashboardSummaryViewModel
{
    public int TotalProducts { get; set; }
    public int ActiveProducts { get; set; }
    public int LowStockProducts { get; set; }
    public int TotalCustomers { get; set; }
    public int TotalInvoices { get; set; }
    public decimal InvoiceRevenueTotal { get; set; }
    public int LowStockThreshold { get; set; } = 10;
    public List<DashboardRecentInvoiceViewModel> RecentInvoices { get; set; } = new();
}

public class DashboardRecentInvoiceViewModel
{
    public int InvoiceId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public decimal GrandTotal { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
}
