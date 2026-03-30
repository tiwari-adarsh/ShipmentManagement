using ShipmentManagement.Models;
using ShipmentManagement.Models.Enums;
using ShipmentManagement.Repositories.Interfaces;
using ShipmentManagement.Services.Interfaces;
using ShipmentManagement.ViewModels.Ship;
using System.Runtime.InteropServices;

namespace ShipmentManagement.Services.Implementations
{
    public class ShipService : IShipService
    {
        private readonly IShipRepository _shipRepository;
        private readonly ILogActionService _logActionService;   

        public ShipService(IShipRepository shipRepository,ILogActionService logActionService)
        {
            _shipRepository = shipRepository;
            _logActionService = logActionService;
        }

        public async Task<ShipListViewModel> GetAllAsync(string? search, string? filterType)
        {
            var ships = await _shipRepository.GetAllAsync();

            //serach 
            if (!string.IsNullOrEmpty(search))
                {
                    ships = ships.Where(s => 
                                        s.ShipName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                        s.ShipCode.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                        s.ImoNumber.Contains(search, StringComparison.OrdinalIgnoreCase)
                                 ).ToList();
            }

            //filter
            if(!string.IsNullOrEmpty(filterType))
            {
                ships = ships.Where(s =>
                                    s.ShipType.Contains(filterType, StringComparison.OrdinalIgnoreCase)
                                    ).ToList();
            }

            return new ShipListViewModel
            {
                Ships = ships,
                TotalShips = ships.Count,
                ActiveShips = ships.Count(s => s.Status == ShipStatus.Active),
                MaintenanceShips = ships.Count(s => s.Status == ShipStatus.Maintenance),
                InactiveShips = ships.Count(s => s.Status == ShipStatus.Inactive),
                SearchTerm = search,
                FilterType = filterType
            };
        }
        public Task<ShipCreateViewModel> GetCreateViewModelAsync()
        {
            return Task.FromResult(new ShipCreateViewModel());
        }
        public async Task<ShipCreateViewModel?> GetEditViewModelAsync(int id)
        {
            try
            {

            var ship = await _shipRepository.GetByIdAsync(id);
            if (ship == null) return null;
            return new ShipCreateViewModel
            {
                Id = ship.Id,
                ShipName = ship.ShipName,
                ShipCode = ship.ShipCode,
                ImoNumber = ship.ImoNumber,
                ShipType = ship.ShipType,
                Status = ship.Status.ToString(),
                CapacityMT = ship.CapacityMT,
                FlagCountry = ship.FlagCountry,
            };
            }
            catch(Exception ex)
            {
                await _logActionService.LogActionAsync("ShipService.GetEditViewModelAsync", "Update", "", $"Error is ID : {id} , message : {ex.Message}");
                return null;
            }
        }
        public async Task<bool> CreateAsync(ShipCreateViewModel model)
        {
            try
            {
                var ship = new Ship
                {
                    ShipCode = model.ShipCode.ToUpper(),
                    ShipName = model.ShipName,
                    ImoNumber = model.ImoNumber,
                    CapacityMT = model.CapacityMT,
                    ShipType = model.ShipType,
                    FlagCountry = model.FlagCountry,
                    YearBuilt = model.YearBuilt,
                    Status = Enum.Parse<ShipStatus>(model.Status),
                    CreatedAt = DateTime.Now,
                };
                await _shipRepository.AddAsync(ship);
                return true;
            }
            catch (Exception ex)
            {
                await _logActionService.LogActionAsync("ShipService.CreateAsync", "Create", "", $"Error is ID : {model.ShipCode} , message : {ex.Message}");
                return false;
            }
        }
        public async Task<bool> UpdateAsync(ShipCreateViewModel model)
        {
            try
            {
                var ship = await _shipRepository.GetByIdAsync(model.Id);

                if (ship == null) return false;
                else
                {
                    ship.ShipCode = model.ShipCode.ToUpper();
                    ship.ShipName = model.ShipName;
                    ship.ImoNumber = model.ImoNumber;
                    ship.CapacityMT = model.CapacityMT;
                    ship.ShipType = model.ShipType;
                    ship.FlagCountry = model.FlagCountry;
                    ship.YearBuilt = model.YearBuilt;
                    ship.Status = Enum.Parse<ShipStatus>(model.Status);
                    ship.UpdatedAt = DateTime.Now;
                    await _shipRepository.UpdateAsync(ship);
                    return true;
                }
            }
            catch (Exception ex)
            {
                await _logActionService.LogActionAsync("ShipService.UpdateAsync", "Updae", "", $"Error is ID : {model.ShipCode} , message : {ex.Message}");
                return false;
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try { 
                var HasShipments = await _shipRepository.HasShipmentsAsync(id);
                if(HasShipments)
                {
                    await _logActionService.LogActionAsync("ShipService.DeleteAsync", "Delete", "", $"Cannot delete ship with ID : {id} because it has associated shipments.");
                    return false;
                }
                else
                {
                    await _shipRepository.DeleteAsync(id); 
                    return true; 
                }
            }
            catch (Exception ex)
            {
                await _logActionService.LogActionAsync("ShipService.DeleteAsync", "Delete", "", $"Error is ID : {id} , message : {ex.Message}");
                return false;
            }
        }

        public async Task<bool> HasActiveShipmentsAsync(int id)
        {
            try
            {
                return await _shipRepository.HasActiveShipmentsAsync(id);
            }
            catch (Exception ex)
            {
                await _logActionService.LogActionAsync("ShipService.HasActiveShipmentsAsync", "Check Active Shipments", "", $"Error is ID : {id} , message : {ex.Message}");
                return false;
            }
        }
    }
}
