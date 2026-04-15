using Microsoft.EntityFrameworkCore;
using Playon24.Domain.Entities;

namespace Playon24.DataAccessLayer.Data
{
    public class Payon24DbContext : DbContext
    {
        public Payon24DbContext(DbContextOptions<Payon24DbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ================= INVOICE =================
            modelBuilder.Entity<Invoice>(e =>
            {
                e.HasKey(x => x.InvoiceID);

                e.Property(x => x.InvoiceNumber)
                    .HasMaxLength(50);

                e.Property(x => x.PaymentStatus)
                    .HasMaxLength(20);

  
                e.Property(x => x.SubTotal).HasPrecision(18, 2);
                e.Property(x => x.TaxAmount).HasPrecision(18, 2);
                e.Property(x => x.DiscountAmount).HasPrecision(18, 2);
                e.Property(x => x.GrandTotal).HasPrecision(18, 2);

                e.HasOne(x => x.Customer)
                    .WithMany()
                    .HasForeignKey(x => x.CustomerID)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(x => x.InvoiceDetails)
                    .WithOne(x => x.Invoice)
                    .HasForeignKey(x => x.InvoiceID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ================= INVOICE DETAIL =================
            modelBuilder.Entity<InvoiceDetail>(e =>
            {
                e.HasKey(x => x.InvoiceDetailID);

 
                e.Property(x => x.UnitPrice).HasPrecision(18, 2);
                e.Property(x => x.LineTotal).HasPrecision(18, 2);
                e.Property(x => x.DiscountPercent).HasPrecision(18, 2);

                e.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ================= PRODUCT =================
            modelBuilder.Entity<Product>(e =>
            {
                e.Property(x => x.ProductName)
                    .HasMaxLength(150);

                e.Property(x => x.ImagePath)
                    .HasMaxLength(300);

           
                e.Property(x => x.UnitPrice)
                    .HasPrecision(18, 2);
            });

            // ================= CUSTOMER =================
            modelBuilder.Entity<Customer>(e =>
            {
                e.Property(x => x.Email)
                    .HasMaxLength(200);
            });
        }
    }
}