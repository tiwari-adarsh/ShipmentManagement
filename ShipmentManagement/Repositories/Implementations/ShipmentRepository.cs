using Microsoft.EntityFrameworkCore;
using ShipmentManagement.Data;
using ShipmentManagement.Models;
using ShipmentManagement.Models.Enums;
using ShipmentManagement.Repositories.Interfaces;

namespace ShipmentManagement.Repositories.Implementations
{
    public class ShipmentRepository : IShipmentRepository
    {
        private readonly AppDbContext _context;

        public ShipmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Shipment>> GetAllAsync()
        {
            return await _context.Shipments
                .Include(s => s.Ship)
                .Include(s => s.Customer)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Shipment>> GetFilteredAsync(
            string? search, string? status, string? type)
        {
            var query = _context.Shipments
                .Include(s => s.Ship)
                .Include(s => s.Customer)
                .AsQueryable();

            // Search by ShipmentCode or CustomerName
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(s =>
                    s.ShipmentCode.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    s.Customer.CustomerName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    s.Customer.CompanyName.Contains(search,StringComparison.OrdinalIgnoreCase));

            // Filter by Status
            if (!string.IsNullOrWhiteSpace(status) &&
                Enum.TryParse<ShipmentStatus>(status, out var statusEnum))
                query = query.Where(s => s.Status == statusEnum);

            // Filter by ShipmentType
            if (!string.IsNullOrWhiteSpace(type) &&
                Enum.TryParse<ShipmentType>(type, out var typeEnum))
                query = query.Where(s => s.ShipmentType == typeEnum);

            return await query
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<Shipment?> GetByIdAsync(int id)
        {
            return await _context.Shipments
                .Include(s => s.Ship)
                .Include(s => s.Customer)
                .Include(s => s.StatusLogs.OrderByDescending(l => l.UpdatedDate))
                .Include(s => s.TrackingSteps.OrderBy(t => t.StepOrder))
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Shipment?> GetByCodeAsync(string code)
        {
            return await _context.Shipments
                .Include(s => s.Ship)
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(s => s.ShipmentCode == code);
        }

        public async Task AddAsync(Shipment shipment)
        {
            await _context.Shipments.AddAsync(shipment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Shipment shipment)
        {
            shipment.UpdatedAt = DateTime.Now;
            _context.Shipments.Update(shipment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var s = await _context.Shipments.FindAsync(id);
            if (s != null)
            {
                _context.Shipments.Remove(s);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetCountByStatusAsync(string status)
        {
            if (!Enum.TryParse<ShipmentStatus>(status, out var statusEnum))
                return 0;
            return await _context.Shipments.CountAsync(s => s.Status == statusEnum);
        }

        public async Task<int> GetTotalCountAsync()
            => await _context.Shipments.CountAsync();

        // Get shipments that are overdue (ArrivalDate in the past but not delivered or delayed)
        public async Task<List<Shipment>> GetOverdueShipmentsAsync()
        {
            return await _context.Shipments
                .Where(s =>
                    s.ArrivalDate.HasValue &&
                    s.ArrivalDate.Value < DateTime.Now &&
                    s.Status != ShipmentStatus.Delivered &&
                    s.Status != ShipmentStatus.Delayed)
                .ToListAsync();
        }

        public async Task<string> GenerateShipmentCodeAsync()
        {
            int year = DateTime.Now.Year;
            int count = await _context.Shipments
                            .CountAsync(s => s.BookingDate.Year == year);
            return $"SHP-{year}-{(count + 1):D4}";
        }

        public async Task AddStatusLogAsync(ShipmentStatusLog log)
        {
            await _context.ShipmentStatusLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task AddTrackingStepAsync(ShipmentTracking step)
        {
            await _context.ShipmentTrackings.AddAsync(step);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Shipment>> GetByStatusAsync(string status)
        {
            if (!Enum.TryParse<ShipmentStatus>(status, true, out var statusEnum))
                return new List<Shipment>();

            return await _context.Shipments
                .Include(s => s.Ship)
                .Include(s => s.Customer)
                .Where(s => s.Status == statusEnum)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<Shipment?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Shipments
                .Include(s => s.Ship)
                .Include(s => s.Customer)
                .Include(s => s.StatusLogs)
                .Include(s => s.TrackingSteps)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}