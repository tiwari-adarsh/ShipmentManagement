using ShipmentManagement.Models;
using ShipmentManagement.Repositories.Interfaces;
using ShipmentManagement.Services.Interfaces;
using ShipmentManagement.ViewModels.Customer;

namespace ShipmentManagement.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepo;

        private readonly ILogActionService _logActionService;
        public CustomerService(ICustomerRepository customerRepo, ILogActionService logActionService )
        {
            _customerRepo = customerRepo;
            _logActionService = logActionService;
        }

        public async Task<CustomerListViewModel> GetAllAsync(string? search)
        {
            var customers = await _customerRepo.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(search))
                customers = customers.Where(c =>
                    c.CustomerName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    c.CompanyName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    c.Email.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    c.CustomerCode.Contains(search, StringComparison.OrdinalIgnoreCase)
                ).ToList();

            return new CustomerListViewModel
            {
                Customers = customers,
                TotalCustomers = customers.Count,
                SearchTerm = search,
            };
        }

        public Task<CustomerCreateViewModel> GetCreateViewModelAsync()
        {
            //clears the form fields when a user clicks "Add New Customer"
            //Task.FromResult to wrap empty object in a "completed" Task
            return Task.FromResult(new CustomerCreateViewModel());   
        }

        public async Task<CustomerCreateViewModel?> GetEditViewModelAsync(int id)
        {
            try
            {
                var c = await _customerRepo.GetByIdAsync(id);
                if (c == null) return null;

                return new CustomerCreateViewModel
                {
                    Id = c.Id,
                    CustomerCode = c.CustomerCode,
                    CustomerName = c.CustomerName,
                    CompanyName = c.CompanyName,
                    Email = c.Email,
                    Phone = c.Phone,
                    Address = c.Address,
                    City = c.City,
                    StateCountry = c.StateCountry,
                };
            }
            catch (Exception ex)
            {
                await _logActionService.LogActionAsync("CustomerService.GetEditViewModelAsync", "Update", "",$"Error fetching customer with ID {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CreateAsync(CustomerCreateViewModel model)
        {
            try
            {
                var customer = new Customer
                {
                    CustomerCode = model.CustomerCode.ToUpper(),
                    CustomerName = model.CustomerName,
                    CompanyName = model.CompanyName,
                    Email = model.Email,
                    Phone = model.Phone,
                    Address = model.Address,
                    City = model.City,
                    StateCountry = model.StateCountry,
                    CreatedAt = DateTime.Now,
                };
                await _customerRepo.AddAsync(customer);
                return true;
            }
            catch (Exception ex) 
            {
                await _logActionService.LogActionAsync("CustomerService.CreateAsync", "Create", model.Email.ToString(), $"Error while creating customer : {ex.Message}");
                return false; 
            }
        }

        public async Task<bool> UpdateAsync(CustomerCreateViewModel model)
        {
            try
            {
                var customer = await _customerRepo.GetByIdAsync(model.Id);
                if (customer == null) return false;

                customer.CustomerCode = model.CustomerCode.ToUpper();
                customer.CustomerName = model.CustomerName;
                customer.CompanyName = model.CompanyName;
                //customer.Email = model.Email;               //Email is intentionally never updated after creation
                customer.Phone = model.Phone;
                customer.Address = model.Address;
                customer.City = model.City;
                customer.StateCountry = model.StateCountry;
                customer.UpdatedAt = DateTime.Now;

                await _customerRepo.UpdateAsync(customer);
                return true;
            }
            catch (Exception ex)
            {
                await _logActionService.LogActionAsync("CustomerService.UpdateAsync", "Update", model.Email.ToString(), $"Error while updating customer with ID {model.Id}: {ex.Message}");
                return false; 
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try { await _customerRepo.DeleteAsync(id); return true; }
            catch (Exception ex)
            { 
                await _logActionService.LogActionAsync("CustomerService.DeleteAsync", "Delete", id.ToString(), $"Error while deleting customer with ID {id}: {ex.Message}");
                return false; 
            }
        }
    }
}