using Microsoft.AspNetCore.Mvc;
using ShipmentManagement.Services.Interfaces;

namespace ShipmentManagement.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _dashboardService.GetDashboardDataAsync();
            return View(model);
        }
    }
}
