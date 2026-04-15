using Microsoft.AspNetCore.Mvc;
using Playon24.BusinessLayer.Exceptions;
using Playon24.BusinessLayer.Modules.Invoices.Interface;
using Playon24.PresentationLayer.Modules.Invoices.Interface;
using Playon24.PresentationLayer.Modules.Invoices.ViewModels;
using Playon24.Web.Reporting;

namespace Playon24.Web.Controllers;

public class InvoiceController : Controller
{
    private readonly IInvoiceViewModelProvider _invoiceViewModelProvider;
    private readonly IInvoiceService _invoiceService;

    public InvoiceController(IInvoiceViewModelProvider invoiceViewModelProvider, IInvoiceService invoiceService)
    {
        _invoiceViewModelProvider = invoiceViewModelProvider;
        _invoiceService = invoiceService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? customerId, string? search)
    {
        var vm = await _invoiceViewModelProvider.GetListPageAsync(customerId, search);
        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Sell()
    {
        var vm = await _invoiceViewModelProvider.GetSellViewModelAsync();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sell(InvoiceSellViewModel model)
    {
        async Task refillLists()
        {
            var template = await _invoiceViewModelProvider.GetSellViewModelAsync();
            model.CustomerOptions = template.CustomerOptions;
            model.ProductOptions = template.ProductOptions;
        }

        if (!ModelState.IsValid)
        {
            await refillLists();
            return View(model);
        }

        try
        {
            var id = await _invoiceViewModelProvider.CreateSaleAsync(model);
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidUserInputException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }

        await refillLists();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> ReportPdf(int id)
    {
        var invoice = await _invoiceService.GetByIdWithDetailsAsync(id);
        if (invoice == null)
            return NotFound();

        try
        {
            var pdf = InvoiceRdlcRenderer.RenderInvoicePdf(invoice);
            var fileName = $"{invoice.InvoiceNumber}.pdf";
            return File(pdf, "application/pdf", fileName);
        }
        catch (Exception ex)
        {
            return BadRequest($"Report error: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var invoice = await _invoiceService.GetByIdWithDetailsAsync(id);
        if (invoice == null)
            return NotFound();
        return View(invoice);
    }
}
