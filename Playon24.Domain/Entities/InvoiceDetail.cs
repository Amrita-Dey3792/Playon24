namespace Playon24.Domain.Entities
{
    public class InvoiceDetail : BaseEntity
    {
        public int InvoiceDetailID { get; set; }

        public int InvoiceID { get; set; }

        public int ProductID { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal DiscountPercent { get; set; } = 0;

        public decimal LineTotal { get; set; }
        public Invoice Invoice { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
