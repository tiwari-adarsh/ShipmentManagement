using Microsoft.EntityFrameworkCore;
using ShipmentManagement.Data;
using ShipmentManagement.Models;
using ShipmentManagement.Repositories.Interfaces;

namespace ShipmentManagement.Repositories.Implementations
{
    public class ShipRepository : IShipRepository
    {
        private readonly AppDbContext _context;
        public ShipRepository(AppDbContext context) { _context = context; }

        public async Task<List<Ship>> GetAllAsync() =>
            await _context.Ships.OrderBy(s => s.ShipName).ToListAsync();

        public async Task<Ship?> GetByIdAsync(int id) =>
            await _context.Ships.FindAsync(id);

        public async Task AddAsync(Ship ship)
        {
            await _context.Ships.AddAsync(ship);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Ship ship)
        {
            ship.UpdatedAt = DateTime.Now;
            _context.Ships.Update(ship);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasShipmentsAsync(int shipId)
        {
            return await _context.Shipments.AnyAsync(s => s.ShipId == shipId);
        }
        public async Task DeleteAsync(int id)
        {
            var ship = await _context.Ships.FindAsync(id);
            if (ship != null) { _context.Ships.Remove(ship); await _context.SaveChangesAsync(); }
        }

        public async Task<bool> HasActiveShipmentsAsync(int id)
        {
            return await _context.Shipments.AnyAsync(s => s.ShipId == id && s.Status != Models.Enums.ShipmentStatus.Delivered);
        }
    }
}