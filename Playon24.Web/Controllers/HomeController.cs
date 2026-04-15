using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Playon24.PresentationLayer.Modules.Dashboard.Interface;
using Playon24.Web.Models;

namespace Playon24.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDashboardViewModelProvider _dashboardViewModelProvider;

        public HomeController(ILogger<HomeController> logger, IDashboardViewModelProvider dashboardViewModelProvider)
        {
            _logger = logger;
            _dashboardViewModelProvider = dashboardViewModelProvider;
        }

        public async Task<IActionResult> Index()
        {
            var vm = await _dashboardViewModelProvider.GetSummaryAsync();
            return View(vm);
        }

        public IActionResult Landing()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
