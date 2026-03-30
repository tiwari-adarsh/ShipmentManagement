using Microsoft.IdentityModel.Tokens;
using ShipmentManagement.Data;
using ShipmentManagement.Models;
using ShipmentManagement.Services.Interfaces;

namespace ShipmentManagement.Services.Implementations
{
    public class LogActionService : ILogActionService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogActionService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogActionAsync(string entity, string action, string entityId, string details)
        {
            try
            {
            var userId = _httpContextAccessor.HttpContext?.User?.Identities?.First().Name ?? string.Empty; 
            var logEntry = new LogHistory
            {
                EntityName = entity,
                Action = action,
                EntityId = entityId,
                Details = details,
                PerformedBy = userId,
                Timestamp = DateTime.UtcNow
            };
            _context.LogHistories.Add(logEntry);
            await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Log Error : " + ex.Message);
            }
        }
    }
}
