using Microsoft.EntityFrameworkCore;
using ShipmentManagement.Data;
using ShipmentManagement.Models;
using ShipmentManagement.Repositories.Interfaces;

namespace ShipmentManagement.Repositories.Implementations
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;
        public CustomerRepository(AppDbContext context) { _context = context; }

        public async Task<List<Customer>> GetAllAsync() =>
            await _context.Customers.OrderBy(c => c.CustomerName).ToListAsync();

        public async Task<Customer?> GetByIdAsync(int id) =>
            await _context.Customers.FindAsync(id);

        public async Task AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer customer)
        {
            customer.UpdatedAt = DateTime.Now;
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            // fast existence check for related shipments
            var hasShipments = await _context.Shipments.AnyAsync(s => s.CustomerId == id);
            if (hasShipments)
                throw new InvalidOperationException("Cannot delete customer with existing shipments.");

            var c = await _context.Customers.FindAsync(id);
            if (c != null)
            {
                _context.Customers.Remove(c);
                await _context.SaveChangesAsync();
            }
        }
    }
}