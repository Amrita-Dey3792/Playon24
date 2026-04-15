using System.ComponentModel.DataAnnotations;

namespace Playon24.PresentationLayer.Modules.Invoices.ViewModels;

public class InvoiceLineInputViewModel
{
    public int ProductId { get; set; }

    public int Quantity { get; set; } = 1;

    [Range(0, 100)]
    public decimal DiscountPercent { get; set; }
}
