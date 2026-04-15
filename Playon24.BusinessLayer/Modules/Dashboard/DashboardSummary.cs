namespace Playon24.BusinessLayer.Modules.Dashboard;

public sealed class DashboardSummary
{
    public int TotalProducts { get; init; }
    public int ActiveProducts { get; init; }
    public int LowStockProducts { get; init; }
    public int TotalCustomers { get; init; }
    public int TotalInvoices { get; init; }
    public decimal InvoiceRevenueTotal { get; init; }
    public IReadOnlyList<DashboardRecentInvoice> RecentInvoices { get; init; } = Array.Empty<DashboardRecentInvoice>();
}

public sealed class DashboardRecentInvoice
{
    public int InvoiceId { get; init; }
    public string InvoiceNumber { get; init; } = string.Empty;
    public string CustomerName { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public decimal GrandTotal { get; init; }
    public string PaymentStatus { get; init; } = string.Empty;
}
