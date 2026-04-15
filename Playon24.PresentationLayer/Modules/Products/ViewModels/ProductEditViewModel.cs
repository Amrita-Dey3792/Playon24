using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Playon24.PresentationLayer.Modules.Products.ViewModels
{
    public class ProductEditViewModel
    {
        public int Id { get; set; }

        [StringLength(150, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 150 characters.")]
        [DisplayName("Product Name")]
        public string ProductName { get; set; } = string.Empty;

        [StringLength(500)]
        [DisplayName("Product Description")]
        public string? Description { get; set; }

        [Range(typeof(decimal), "0.01", "999999999", ErrorMessage = "Price must be greater than zero.")]
        [DisplayName("Unit Price")]
        public decimal UnitPrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
        [DisplayName("Stock Quantity")]
        public int StockQuantity { get; set; }

        [DisplayName("Active")]
        public bool IsActive { get; set; }

        [DisplayName("Current image")]
        public string? CurrentImagePath { get; set; }

        [DisplayName("Product Image")]
        public IFormFile? ImageFile { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
