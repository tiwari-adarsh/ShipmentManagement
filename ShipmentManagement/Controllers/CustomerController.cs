using Microsoft.AspNetCore.Mvc;
using ShipmentManagement.Services.Interfaces;
using ShipmentManagement.ViewModels.Customer;

namespace ShipmentManagement.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IActionResult> Index(string? search)
        {
            ViewData["Title"] = "Customer Master";
            var model = await _customerService.GetAllAsync(search);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()    //blank form
        {
            ViewData["Title"] = "Add Customer";
            var model = await _customerService.GetCreateViewModelAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Add Customer";
                return View(model);
            }

            var success = await _customerService.CreateAsync(model);
            if (success)
            {
                TempData["Success"] = $"Customer '{model.CustomerName}' added successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Failed to add customer. Email or Code may already exist.";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Edit Customer";
            var model = await _customerService.GetEditViewModelAsync(id);
            if (model == null) return NotFound();
            return View("Create",model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Edit Customer";
                return View("Create", model);
            }

            var success = await _customerService.UpdateAsync(model);
            if (success)
            {
                TempData["Success"] = $"Customer '{model.CustomerName}' updated!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Failed to update customer.";
            return View("Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _customerService.DeleteAsync(id);
            TempData[success ? "Success" : "Error"] = success
                ? "Customer deleted successfully!"
                : "Cannot delete — customer may have active shipments.";
            return RedirectToAction(nameof(Index));
        }
    }
}