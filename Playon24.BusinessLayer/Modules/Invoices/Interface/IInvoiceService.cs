using Playon24.Domain.Entities;

namespace Playon24.BusinessLayer.Modules.Invoices.Interface;

public interface IInvoiceService
{
    Task<Invoice?> GetByIdWithDetailsAsync(int invoiceId);
    Task<IReadOnlyList<Invoice>> GetListAsync(int? customerId, string? search);
    Task<Invoice> CreateSaleAsync(int customerId, decimal discountAmount, decimal taxAmount, string paymentStatus,
        IReadOnlyList<InvoiceLineRequest> lines);
}

public sealed record InvoiceLineRequest(int ProductId, int Quantity, decimal DiscountPercent);
