namespace Playon24.DataAccessLayer.Modules.Dashboard;

public sealed class DashboardSummaryData
{
    public int TotalProducts { get; init; }
    public int ActiveProducts { get; init; }
    public int LowStockProducts { get; init; }
    public int TotalCustomers { get; init; }
    public int TotalInvoices { get; init; }
    public decimal InvoiceRevenueTotal { get; init; }
    public IReadOnlyList<RecentInvoiceRowData> RecentInvoices { get; init; } = Array.Empty<RecentInvoiceRowData>();
}

public sealed class RecentInvoiceRowData
{
    public int InvoiceId { get; init; }
    public string InvoiceNumber { get; init; } = string.Empty;
    public string CustomerName { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public decimal GrandTotal { get; init; }
    public string PaymentStatus { get; init; } = string.Empty;
}
