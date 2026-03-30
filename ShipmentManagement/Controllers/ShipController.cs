using Microsoft.AspNetCore.Mvc;
using ShipmentManagement.Models.Enums;
using ShipmentManagement.Services.Interfaces;
using ShipmentManagement.ViewModels.Ship;

namespace ShipmentManagement.Controllers
{
    public class ShipController : Controller
    {
        private readonly IShipService _shipService;

        public ShipController(IShipService shipService)
        {
            _shipService = shipService;
        }

        public async Task<IActionResult> Index(string? search, string? filterType)
        {
            ViewData["Title"] = "Ship Master";
            var model = await _shipService.GetAllAsync(search, filterType);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()   //blank form
        {
            ViewData["Title"] = "Add New Ship";
            var model = await _shipService.GetCreateViewModelAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShipCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Add New Ship";
                return View(model);
            }

            var success = await _shipService.CreateAsync(model);
            if (success)
            {
                TempData["Success"] = $"Ship '{model.ShipName}' added successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Failed to add ship. IMO or Ship Code may already exist.";
            ViewData["Title"] = "Add New Ship";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Edit Ship";
            var model = await _shipService.GetEditViewModelAsync(id);
            if (model == null) return NotFound();
            return View("Create",model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ShipCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Edit Ship";
                return View("Create",model);
            }

            if (Enum.Parse<ShipStatus>(model.Status) != ShipStatus.Active) // if status is not active, check if it has shipments
            {
                var hasActiveShipments = await _shipService.HasActiveShipmentsAsync(model.Id);
                if (hasActiveShipments)
                {
                    TempData["Error"] = "Cannot update ship status, it have active shipments.";
                    return View("Create", model);
                }
            }

            var success = await _shipService.UpdateAsync(model);
            if (success)
            {
                TempData["Success"] = $"Ship '{model.ShipName}' updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Failed to update ship.";
            return View("Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _shipService.DeleteAsync(id);
            TempData[success ? "Success" : "Error"] = success
                ? "Ship deleted successfully!"
                : "Cannot delete ship, it may have active shipments.";
            return RedirectToAction(nameof(Index));
        }
    }
}