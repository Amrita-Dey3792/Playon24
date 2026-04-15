using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Playon24.PresentationLayer.Modules.Invoices.ViewModels;

public class InvoiceSellViewModel : IValidatableObject
{
    [Display(Name = "Customer")]
    [Range(1, int.MaxValue, ErrorMessage = "Select a customer.")]
    public int CustomerId { get; set; }

    [Display(Name = "Invoice discount")]
    [Range(0, double.MaxValue)]
    public decimal DiscountAmount { get; set; }

    [Display(Name = "Tax")]
    [Range(0, double.MaxValue)]
    public decimal TaxAmount { get; set; }

    [StringLength(20)]
    public string PaymentStatus { get; set; } = "Paid";

    public List<InvoiceLineInputViewModel> Lines { get; set; } = new();

    public List<CustomerSelectOption> CustomerOptions { get; set; } = new();
    public List<ProductSelectOption> ProductOptions { get; set; } = new();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var hasLine = false;
        for (var i = 0; i < Lines.Count; i++)
        {
            var line = Lines[i];
            if (line.ProductId <= 0 && line.Quantity <= 0 && line.DiscountPercent == 0)
                continue;

            if (line.ProductId > 0 && line.Quantity < 1)
                yield return new ValidationResult("Quantity must be at least 1.", [$"Lines[{i}].Quantity"]);

            if (line.Quantity > 0 && line.ProductId <= 0)
                yield return new ValidationResult("Select a product.", [$"Lines[{i}].ProductId"]);

            if (line.ProductId > 0 && line.Quantity > 0)
                hasLine = true;
        }

        if (!hasLine)
            yield return new ValidationResult("Add at least one product line with quantity.", [nameof(Lines)]);
    }
}

public class CustomerSelectOption
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
}

public class ProductSelectOption
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
}
