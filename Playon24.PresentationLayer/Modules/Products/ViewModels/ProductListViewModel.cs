namespace Playon24.PresentationLayer.Modules.Products.ViewModels
{
    public class ProductListViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal UnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public string ImagePath { get; set; } = string.Empty;
    }
}
