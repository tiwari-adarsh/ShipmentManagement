using Microsoft.EntityFrameworkCore;
using ShipmentManagement.Data;
using ShipmentManagement.Models;
using ShipmentManagement.Repositories.Interfaces;
using ShipmentManagement.Models.Enums;

namespace ShipmentManagement.Repositories.Implementations
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _context;

        public DashboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardStat> GetStatsAsync()
        {
            return new DashboardStat
            {
                TotalShipments = await _context.Shipments.CountAsync(),
                Upcoming = await _context.Shipments.CountAsync(s => s.Status == ShipmentStatus.Pending),
                Ongoing = await _context.Shipments.CountAsync(s => s.Status == ShipmentStatus.InTransit),
                Delivered = await _context.Shipments.CountAsync(s => s.Status == ShipmentStatus.Delivered),
                Delayed = await _context.Shipments.CountAsync(s => s.Status == ShipmentStatus.Delayed),
            };
        }

        public async Task<List<(string Month, int Count, int Delivered)>> GetMonthlyTrendAsync()
        {
            var data = await _context.Shipments
                .GroupBy(s => new { s.BookingDate.Year, s.BookingDate.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    Count = g.Count(),
                    Delivered = g.Count(s => s.Status == ShipmentStatus.Delivered)
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .Take(12)
                .ToListAsync();

            return data.Select(d => (
                new DateTime(d.Year, d.Month, 1).ToString("MMM"),
                d.Count,
                d.Delivered
            )).ToList();
        }

        public async Task<List<(ShipmentStatus Status, int Count)>> GetStatusDistributionAsync()
        {
            var data = await _context.Shipments
                .GroupBy(s => s.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            return data.Select(d => (d.Status, d.Count)).ToList();
        }

        public async Task<List<(string ShipName, int Count)>> GetShipWiseCountAsync()
        {
            var data = await _context.Shipments
                .Include(s => s.Ship)
                .GroupBy(s => s.Ship.ShipName)
                .Select(g => new { ShipName = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(6)
                .ToListAsync();

            return data.Select(d => (d.ShipName, d.Count)).ToList();
        }
    }
}