using Microsoft.AspNetCore.Mvc;
using Playon24.BusinessLayer.Exceptions;
using Playon24.PresentationLayer.Modules.Customers.Interface;
using Playon24.PresentationLayer.Modules.Customers.ViewModels;

namespace Playon24.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerViewModelProvider _customerViewModelProvider;

        public CustomerController(ICustomerViewModelProvider customerViewModelProvider)
        {
            _customerViewModelProvider = customerViewModelProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var list = await _customerViewModelProvider.GetAllAsync();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var viewModel = await _customerViewModelProvider.GetDetailsByIdAsync(id);
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
        public async Task<IActionResult> Create(CustomerCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _customerViewModelProvider.AddAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidUserInputException ex)
            {
                ModelState.AddModelError(nameof(model.Email), ex.Message);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var vm = await _customerViewModelProvider.GetDetailsByIdAsync(id);
            if (vm == null)
                return NotFound();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromRoute] int id, CustomerEditViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _customerViewModelProvider.UpdateAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
            catch (InvalidUserInputException ex)
            {
                ModelState.AddModelError(nameof(model.Email), ex.Message);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var viewModel = await _customerViewModelProvider.GetDetailsByIdAsync(id);
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
                var deleted = await _customerViewModelProvider.DeleteAsync(id);
                if (deleted)
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
