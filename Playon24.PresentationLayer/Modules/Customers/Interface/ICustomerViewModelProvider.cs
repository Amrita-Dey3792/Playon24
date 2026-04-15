using Playon24.PresentationLayer.Modules.Customers.ViewModels;

namespace Playon24.PresentationLayer.Modules.Customers.Interface
{
    public interface ICustomerViewModelProvider
    {
        Task AddAsync(CustomerCreateViewModel model);
        Task<CustomerEditViewModel?> GetDetailsByIdAsync(int id);
        Task<IReadOnlyList<CustomerListViewModel>> GetAllAsync();
        Task UpdateAsync(CustomerEditViewModel model);
        Task<bool> DeleteAsync(int id);
    }
}
