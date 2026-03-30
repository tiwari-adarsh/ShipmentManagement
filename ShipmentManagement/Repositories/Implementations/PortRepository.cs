using Microsoft.EntityFrameworkCore;
using ShipmentManagement.Data;
using ShipmentManagement.Models;
using ShipmentManagement.Repositories.Interfaces;

namespace ShipmentManagement.Repositories.Implementations
{
    public class PortRepository : IPortRepository
    {
        private readonly AppDbContext _context;
        public PortRepository(AppDbContext context) { _context = context; }

        public async Task<List<Port>> GetAllActiveAsync() =>
            await _context.Ports.Where(p => p.IsActive)
                                .OrderBy(p => p.PortName)
                                .ToListAsync();
    }
}