using Playon24.Domain.Entities;

namespace Playon24.DataAccessLayer.Modules.Customers.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> AddAsync(Customer customer);
        Task<Customer?> GetByIdAsync(int id);
        Task<Customer?> GetByEmailAsync(string email);
        Task<IReadOnlyList<Customer>> GetAllAsync();
        Task UpdateAsync(Customer customer);
        Task<bool> DeleteAsync(Customer customer);
    }
}
