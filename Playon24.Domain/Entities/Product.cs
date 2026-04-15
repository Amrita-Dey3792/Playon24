using System.ComponentModel.DataAnnotations;

namespace Playon24.Domain.Entities
{
    public class Product : BaseEntity
    {
        public int Id { get; set; }


        [StringLength(150)]
        public string ProductName { get; set; }


        [StringLength(500)]
        public string? Description { get; set; }

        public decimal UnitPrice { get; set; }

        public int StockQuantity { get; set; }

        [StringLength(300)]
        public string ImagePath { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
