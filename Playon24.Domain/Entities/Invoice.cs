using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Playon24.Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public int InvoiceID { get; set; }

        public int CustomerID { get; set; }

        [StringLength(50)]
        public string InvoiceNumber { get; set; }

        public decimal SubTotal { get; set; }

        public decimal DiscountAmount { get; set; } = 0;

        public decimal TaxAmount { get; set; } = 0;

        public decimal GrandTotal { get; set; }

        [StringLength(20)]
        public string PaymentStatus { get; set; } = "Paid";

        public Customer Customer { get; set; } = null!;
        public ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
    }
}
