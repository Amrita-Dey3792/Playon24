using System.Reflection;
using Microsoft.Reporting.NETCore;
using Playon24.Domain.Entities;
using Playon24.Web.Reports;

namespace Playon24.Web.Reporting;

public static class InvoiceRdlcRenderer
{
    public static byte[] RenderInvoicePdf(Invoice invoice)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = assembly.GetManifestResourceNames()
            .FirstOrDefault(n => n.EndsWith("InvoiceReport.rdlc", StringComparison.OrdinalIgnoreCase));
        if (resourceName == null)
            throw new InvalidOperationException("Embedded report InvoiceReport.rdlc was not found.");

        var items = invoice.InvoiceDetails
            .OrderBy(d => d.InvoiceDetailID)
            .Select(d => new InvoiceReportItem
            {
                Description = d.Product?.ProductName ?? $"Product #{d.ProductID}",
                Price = d.UnitPrice,
                Qty = d.Quantity,
                Total = d.LineTotal
            })
            .ToList();

        var customer = invoice.Customer;
        var customerName = customer != null
            ? $"{customer.FirstName} {customer.LastName}".Trim()
            : $"Customer #{invoice.CustomerID}";

        var title =
            $"Invoice {invoice.InvoiceNumber} | {customerName} | {invoice.CreatedAt:yyyy-MM-dd HH:mm} UTC | " +
            $"Sub {invoice.SubTotal:0.00} | Disc {invoice.DiscountAmount:0.00} | Tax {invoice.TaxAmount:0.00} | " +
            $"Total {invoice.GrandTotal:0.00} | {invoice.PaymentStatus}";

        using var stream = assembly.GetManifestResourceStream(resourceName)
                           ?? throw new InvalidOperationException("Could not open InvoiceReport.rdlc stream.");

        using var report = new LocalReport();
        report.LoadReportDefinition(stream);
        report.DataSources.Add(new ReportDataSource("Items", items));
        report.SetParameters(new ReportParameter("Title", title));

        var result = report.Render("PDF");
        return result;
    }
}
