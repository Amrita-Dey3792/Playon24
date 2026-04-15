using AutoMapper;
using Playon24.Domain.Entities;
using Playon24.PresentationLayer.Modules.Products.ViewModels;


namespace Playon24.PresentationLayer.Modules.Products
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            // Entity to ViewModel
            CreateMap<Product, ProductEditViewModel>()
                .ForMember(dest => dest.CurrentImagePath, opt => opt.MapFrom(src => src.ImagePath));

            CreateMap<Product, ProductListViewModel>();


            // ViewModel to Entity
            CreateMap<ProductCreateViewModel, Product>();
            CreateMap<ProductEditViewModel, Product>();

        }
    }
}
