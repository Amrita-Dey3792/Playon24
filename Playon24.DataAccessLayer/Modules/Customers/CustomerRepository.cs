using Microsoft.EntityFrameworkCore;
using Playon24.DataAccessLayer.Data;
using Playon24.DataAccessLayer.Modules.Customers.Interfaces;
using Playon24.Domain.Entities;

namespace Playon24.DataAccessLayer.Modules.Customers
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly Payon24DbContext _dbContext;

        public CustomerRepository(Payon24DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            await _dbContext.Customers.AddAsync(customer);
            await _dbContext.SaveChangesAsync();
            return customer;
        }

        public async Task<IReadOnlyList<Customer>> GetAllAsync()
        {
            return await _dbContext.Customers
                .AsNoTracking()
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToListAsync();
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await _dbContext.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _dbContext.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> DeleteAsync(Customer customer)
        {
            _dbContext.Customers.Remove(customer);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task UpdateAsync(Customer customer)
        {
            _dbContext.Customers.Update(customer);
            await _dbContext.SaveChangesAsync();
        }
    }
}
