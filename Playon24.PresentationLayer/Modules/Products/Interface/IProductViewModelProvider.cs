using Playon24.PresentationLayer.Modules.Products.ViewModels;

namespace Playon24.PresentationLayer.Modules.Products.Interface
{
    public interface IProductViewModelProvider
    {
        Task AddAsync(ProductCreateViewModel productCvm);
        Task<ProductEditViewModel?> GetDetailsByIdAsync(int id);
        Task<IReadOnlyList<ProductListViewModel>> GetAllAsync();
        Task UpdateAsync(ProductEditViewModel productEvm);
        Task<bool> DeleteAsync(int id);
    }
}
