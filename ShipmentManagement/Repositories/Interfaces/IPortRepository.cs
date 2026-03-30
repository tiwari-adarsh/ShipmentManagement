using ShipmentManagement.Models;

namespace ShipmentManagement.Repositories.Interfaces
{
    public interface IPortRepository
    {
        Task<List<Port>> GetAllActiveAsync();
    }
}