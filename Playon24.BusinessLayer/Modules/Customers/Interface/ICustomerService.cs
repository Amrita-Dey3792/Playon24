using Playon24.Domain.Entities;

namespace Playon24.BusinessLayer.Modules.Customers.Interface
{
    public interface ICustomerService
    {
        Task<Customer> AddAsync(Customer customer);
        Task<Customer?> GetByIdAsync(int id);
        Task<IReadOnlyList<Customer>> GetAllAsync();
        Task UpdateAsync(Customer customer);
        Task<bool> DeleteAsync(int id);
    }
}
