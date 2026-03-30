namespace ShipmentManagement.Services.Interfaces
{
    public interface ILogActionService
    {
        Task LogActionAsync(string entity, string action, string entityId, string details);
    }
}
