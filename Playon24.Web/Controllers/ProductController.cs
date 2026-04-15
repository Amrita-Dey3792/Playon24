using Microsoft.AspNetCore.Mvc;
using Playon24.BusinessLayer.Exceptions;
using Playon24.PresentationLayer.Modules.Products.Interface;
using Playon24.PresentationLayer.Modules.Products.ViewModels;

namespace Playon24.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductViewModelProvider _productViewModelProvider;

        public ProductController(IProductViewModelProvider productViewModelProvider)
        {
            _productViewModelProvider = productViewModelProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productViewModelProvider.GetAllAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var viewModel = await _productViewModelProvider.GetDetailsByIdAsync(id);
            if (viewModel == null)
                return NotFound();

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel productCvm)
        {
            if (!ModelState.IsValid)
                return View(productCvm);

            try
            {
                await _productViewModelProvider.AddAsync(productCvm);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidUserInputException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (FileTypeException ex)
            {
                ModelState.AddModelError(nameof(productCvm.ImageFile), ex.Message);
            }
            catch (EmptyFileException ex)
            {
                ModelState.AddModelError(nameof(productCvm.ImageFile), ex.Message);
            }
            catch (FileSizeExceedException ex)
            {
                ModelState.AddModelError(nameof(productCvm.ImageFile), ex.Message);
            }

            return View(productCvm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var productEditViewModel = await _productViewModelProvider.GetDetailsByIdAsync(id);
            if (productEditViewModel == null)
                return NotFound();

            return View(productEditViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromRoute] int id, ProductEditViewModel productEvm)
        {
            if (id != productEvm.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(productEvm);

            try
            {
                await _productViewModelProvider.UpdateAsync(productEvm);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
            catch (InvalidUserInputException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (FileTypeException ex)
            {
                ModelState.AddModelError(nameof(productEvm.ImageFile), ex.Message);
            }
            catch (EmptyFileException ex)
            {
                ModelState.AddModelError(nameof(productEvm.ImageFile), ex.Message);
            }
            catch (FileSizeExceedException ex)
            {
                ModelState.AddModelError(nameof(productEvm.ImageFile), ex.Message);
            }

            return View(productEvm);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var viewModel = await _productViewModelProvider.GetDetailsByIdAsync(id);

            if (viewModel == null)
                return NotFound();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var isDeleted = await _productViewModelProvider.DeleteAsync(id);
                if (isDeleted)
                    return RedirectToAction(nameof(Index));
                return NotFound();
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
    }
}
