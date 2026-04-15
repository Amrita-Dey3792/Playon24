using Playon24.Domain.Entities;

namespace Playon24.DataAccessLayer.Modules.Invoices.Interfaces;

public interface IInvoiceRepository
{
    Task<Invoice?> GetByIdWithDetailsAsync(int invoiceId);
    Task<IReadOnlyList<Invoice>> GetListAsync(int? customerId, string? search);
    Task<Invoice> CreateSaleAsync(Invoice invoice, IReadOnlyList<InvoiceDetail> details);
}
