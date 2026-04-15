using AutoMapper;
using Playon24.Domain.Entities;
using Playon24.PresentationLayer.Modules.Customers.ViewModels;

namespace Playon24.PresentationLayer.Modules.Customers
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {

            // ViewModel -> Entity
            CreateMap<CustomerCreateViewModel, Customer>();
           

            // Entity -> ViewModel
            CreateMap<Customer, CustomerListViewModel>();

            // View Model <-> Entity
            CreateMap<CustomerEditViewModel, Customer>().ReverseMap();

        }
    }
}
