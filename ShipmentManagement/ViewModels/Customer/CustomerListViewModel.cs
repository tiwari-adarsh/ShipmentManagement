using ShipmentManagement.Models;

namespace ShipmentManagement.ViewModels.Customer
{
    public class CustomerListViewModel
    {
        public List<Models.Customer> Customers { get; set; } = new();
        public int TotalCustomers { get; set; }
        public string? SearchTerm { get; set; }
    }
}