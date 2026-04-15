using AutoMapper;
using Playon24.BusinessLayer.Modules.Customers.Interface;
using Playon24.Domain.Entities;
using Playon24.PresentationLayer.Modules.Customers.Interface;
using Playon24.PresentationLayer.Modules.Customers.ViewModels;

namespace Playon24.PresentationLayer.Modules.Customers
{
    public class CustomerViewModelProvider : ICustomerViewModelProvider
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomerViewModelProvider(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        public async Task AddAsync(CustomerCreateViewModel customerCVM)
        {
            var customer = _mapper.Map<Customer>(customerCVM);
            await _customerService.AddAsync(customer);
        }

        public async Task<IReadOnlyList<CustomerListViewModel>> GetAllAsync()
        {
            var customers = await _customerService.GetAllAsync();
            return _mapper.Map<IReadOnlyList<CustomerListViewModel>>(customers);
        }

        public async Task<CustomerEditViewModel?> GetDetailsByIdAsync(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            return customer == null ? null : _mapper.Map<CustomerEditViewModel>(customer);
        }

        public async Task UpdateAsync(CustomerEditViewModel customerEVM)
        {
            var customer = _mapper.Map<Customer>(customerEVM);
            await _customerService.UpdateAsync(customer);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _customerService.DeleteAsync(id);
        }

    }
}
