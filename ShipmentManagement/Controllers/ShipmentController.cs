using Microsoft.AspNetCore.Mvc;
using ShipmentManagement.Services.Interfaces;
using ShipmentManagement.ViewModels.Shipment;

namespace ShipmentManagement.Controllers
{
    public class ShipmentController : Controller
    {
        private readonly IShipmentService _shipmentService;

        public ShipmentController(IShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        //Shipment
        public async Task<IActionResult> Index(string? search, string? filterStatus, string? filterType)
        {
            ViewData["Title"] = "Shipment List";
            var model = await _shipmentService.GetAllAsync(
                search, filterStatus, filterType);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Title"] = "Shipment Booking";
            var model = await _shipmentService.GetCreateViewModelAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShipmentCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var fresh = await _shipmentService.GetCreateViewModelAsync();
                model.Ships = fresh.Ships;
                model.Customers = fresh.Customers;
                model.Ports = fresh.Ports;
                model.ShipmentCode = fresh.ShipmentCode;
                ViewData["Title"] = "Shipment Booking";
                return View(model);
            }

            var success = await _shipmentService.CreateShipmentAsync(model);
            if (success)
            {
                TempData["Success"] = "Shipment booked successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Something went wrong. Please try again.";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            ViewData["Title"] = "Shipment Details";
            var model = await _shipmentService.GetDetailsAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            ViewData["Title"] = "Update Status";
            var model = await _shipmentService.GetStatusUpdateViewModelAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(ShipmentStatusUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Update Status";
                return View(model);
            }

            var success = await _shipmentService.UpdateStatusAsync(model);
            if (success)
            {
                TempData["Success"] =
                    $"Status updated to '{model.NewStatus}' successfully!";
                return RedirectToAction(nameof(Details), new { id = model.Id });
            }

            TempData["Error"] = "Failed to update status.";
            return View(model);
        }
    }
}