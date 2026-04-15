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
            modelBuilder.Entity<Invoice>(e =>
            {
                e.HasKey(x => x.InvoiceID);
                e.Property(x => x.InvoiceNumber).HasMaxLength(50);
                e.Property(x => x.PaymentStatus).HasMaxLength(20);
                e.HasOne(x => x.Customer)
                    .WithMany()
                    .HasForeignKey(x => x.CustomerID)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasMany(x => x.InvoiceDetails)
                    .WithOne(x => x.Invoice)
                    .HasForeignKey(x => x.InvoiceID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<InvoiceDetail>(e =>
            {
                e.HasKey(x => x.InvoiceDetailID);
                e.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Product>(e =>
            {
                e.Property(x => x.ProductName).HasMaxLength(150);
                e.Property(x => x.ImagePath).HasMaxLength(300);
            });

            modelBuilder.Entity<Customer>(e =>
            {
                e.Property(x => x.Email).HasMaxLength(200);
            });
        }
    }
}
