using Playon24.BusinessLayer.Exceptions;
using Playon24.BusinessLayer.Modules.Customers.Interface;
using Playon24.DataAccessLayer.Modules.Customers.Interfaces;
using Playon24.Domain.Entities;

namespace Playon24.BusinessLayer.Modules.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;

        public CustomerService(ICustomerRepository repo)
        {
            _repo = repo;
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            var now = DateTime.UtcNow;
            NormalizeCustomer(customer);
            customer.CreatedAt = now;
            customer.UpdatedAt = now;
            await EnsureEmailIsUnique(customer.Email, null);
            return await _repo.AddAsync(customer);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await GetCustomerOrThrow(id);
            return await _repo.DeleteAsync(customer);
        }

        public Task<IReadOnlyList<Customer>> GetAllAsync()
        {
            return _repo.GetAllAsync();
        }

        public Task<Customer?> GetByIdAsync(int id)
        {
            return _repo.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Customer customer)
        {
            var existing = await GetCustomerOrThrow(customer.Id);
            NormalizeCustomer(customer);
            customer.CreatedAt = existing.CreatedAt;
            customer.UpdatedAt = DateTime.UtcNow;
            await EnsureEmailIsUnique(customer.Email, customer.Id);
            await _repo.UpdateAsync(customer);
        }

        private async Task EnsureEmailIsUnique(string email, int? excludedId)
        {
            var other = await _repo.GetByEmailAsync(email);
            if (other != null && other.Id != excludedId)
                throw new InvalidUserInputException("A customer with this email already exists.");
        }

        private static void NormalizeCustomer(Customer customer)
        {
            customer.FirstName = customer.FirstName.Trim();
            customer.LastName = customer.LastName.Trim();
            customer.Email = customer.Email.Trim();
            customer.Phone = customer.Phone.Trim();
            customer.Address = customer.Address.Trim();
            customer.City = customer.City.Trim();
        }

        private async Task<Customer> GetCustomerOrThrow(int id)
        {
            var customer = await _repo.GetByIdAsync(id);
            if (customer == null)
                throw new InvalidOperationException("Customer not found.");

            return customer;
        }
    }
}
