using ShipmentManagement.ViewModels.Customer;

namespace ShipmentManagement.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerListViewModel> GetAllAsync(string? search);
        Task<CustomerCreateViewModel> GetCreateViewModelAsync();
        Task<CustomerCreateViewModel?> GetEditViewModelAsync(int id);
        Task<bool> CreateAsync(CustomerCreateViewModel model);
        Task<bool> UpdateAsync(CustomerCreateViewModel model);
        Task<bool> DeleteAsync(int id);
    }
}