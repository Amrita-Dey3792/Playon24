namespace Playon24.Web.Reports;

public sealed class InvoiceReportItem
{
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Qty { get; set; }
    public decimal Total { get; set; }
}
