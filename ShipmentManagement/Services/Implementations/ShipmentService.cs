using Microsoft.AspNetCore.Mvc.Rendering;
using ShipmentManagement.Models;
using ShipmentManagement.Models.Enums;
using ShipmentManagement.Repositories.Interfaces;
using ShipmentManagement.Services.Interfaces;
using ShipmentManagement.ViewModels.Shipment;
using static ShipmentManagement.ViewModels.Shipment.ShipmentCreateViewModel;

namespace ShipmentManagement.Services.Implementations
{
    public class ShipmentService : IShipmentService
    {
        private readonly IShipmentRepository _shipmentRepo;
        private readonly IShipRepository _shipRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IPortRepository _portRepo;
        private readonly ILogActionService _logActionService;
        public ShipmentService(IShipmentRepository shipmentRepo,IShipRepository shipRepo,
                               ICustomerRepository customerRepo,IPortRepository portRepo,
                               ILogActionService logActionService
            )
        {
            _shipmentRepo = shipmentRepo;
            _shipRepo = shipRepo;
            _customerRepo = customerRepo;
            _portRepo = portRepo;
            _logActionService = logActionService;
        }

        public async Task<ShipmentCreateViewModel?> GetCreateViewModelAsync()
        {
            try
            {
                var ships = await _shipRepo.GetAllAsync();
                var customers = await _customerRepo.GetAllAsync();
                var ports = await _portRepo.GetAllActiveAsync();
                var code = await _shipmentRepo.GenerateShipmentCodeAsync();

                return new ShipmentCreateViewModel
                {
                    ShipmentCode = code,
                    BookingDate = DateTime.Today,

                    Ships = ships.Select(s => new SelectListItem($"{s.ShipName} ({s.ShipCode}) — {s.CapacityMT:N0} MT", s.Id.ToString())
                                        ).ToList(),

                    Customers = customers.Select(c => new SelectListItem($"{c.CustomerName} — {c.CompanyName}", c.Id.ToString())
                                                ).ToList(),

                    Ports = ports.Select(p => new SelectListItem($"{p.PortName} ({p.PortCode})", p.PortName)
                                        ).ToList(),

                    //  customer data for JS auto-fill
                    CustomerDataList = customers.Select(c => new CustomerDropdownItem
                    {
                        Id = c.Id.ToString(),
                        Name = c.CustomerName,
                        Company = c.CompanyName,
                        Email = c.Email,
                        Phone = c.Phone,
                    }).ToList(),
                };

            }
            catch (Exception ex)
            {
                await _logActionService.LogActionAsync("ShipmentService.GetCreateViewModelAsync","Crate","",$"Erorr in this : { ex.Message}");
                return null;
            }
        }

        public async Task<bool> CreateShipmentAsync(ShipmentCreateViewModel model)
        {
            try
            {
                var code = await _shipmentRepo.GenerateShipmentCodeAsync();

                var shipment = new Shipment
                {
                    ShipmentCode = code,
                    BookingDate = model.BookingDate,
                    ShipmentType = Enum.Parse<ShipmentType>(model.ShipmentType),
                    CargoType = Enum.Parse<CargoType>(model.CargoType),
                    WeightMT = model.WeightMT,
                    ContainerNumber = model.ContainerNumber,
                    ShipId = model.ShipId,
                    CustomerId = model.CustomerId,
                    SourcePort = model.SourcePort,
                    DestinationPort = model.DestinationPort,
                    DepartureDate = model.DepartureDate,
                    ArrivalDate = model.ArrivalDate,
                    SpecialInstructions = model.SpecialInstructions,
                    Status = ShipmentStatus.Pending,
                    CreatedAt = DateTime.Now,
                };

                await _shipmentRepo.AddAsync(shipment);

                // Add first tracking step — Booking Confirmed
                await _shipmentRepo.AddTrackingStepAsync(new ShipmentTracking
                {
                    ShipmentId = shipment.Id,
                    StepTitle = "Booking Confirmed",
                    StepLocation = model.SourcePort,
                    StepDate = DateTime.Now,
                    StepStatus = "done",
                    StepOrder = 1,
                });

                // Add initial status log
                await _shipmentRepo.AddStatusLogAsync(new ShipmentStatusLog
                {
                    ShipmentId = shipment.Id,
                    Status = "Pending",
                    Remarks = "Shipment booking created",
                    UpdatedDate = DateTime.Now,
                    UpdatedBy = "System",
                });

                return true;
            }
            catch (Exception ex)
            {
                await _logActionService.LogActionAsync("ShipmentService.CreateShipmentAsync", "Crate", "", $"Erorr in this : {ex.Message}");
                return false;
            }
        }
        public async Task<ShipmentListViewModel> GetAllAsync(
            string? search, string? status, string? type)
        {
            // Run auto-delay check every time list loaded
            await AutoMarkDelayedAsync();

            var shipments = await _shipmentRepo.GetFilteredAsync(search, status, type);
            var all = await _shipmentRepo.GetAllAsync();

            return new ShipmentListViewModel
            {
                Shipments = shipments,
                TotalCount = all.Count,
                PendingCount = all.Count(s => s.Status == ShipmentStatus.Pending),
                InTransitCount = all.Count(s => s.Status == ShipmentStatus.InTransit),
                DeliveredCount = all.Count(s => s.Status == ShipmentStatus.Delivered),
                DelayedCount = all.Count(s => s.Status == ShipmentStatus.Delayed),
                SearchTerm = search,
                FilterStatus = status,
                FilterType = type,
            };
        }

        public async Task<ShipmentDetailsViewModel?> GetDetailsAsync(int id)
        {
            var shipment = await _shipmentRepo.GetByIdAsync(id);
            if (shipment == null) return null;

            return new ShipmentDetailsViewModel
            {
                Shipment = shipment,
                TrackingSteps = shipment.TrackingSteps
                                    .OrderBy(t => t.StepOrder).ToList(),
                StatusLogs = shipment.StatusLogs
                                    .OrderByDescending(l => l.UpdatedDate).ToList(),
            };
        }
        public async Task<ShipmentStatusUpdateViewModel?> GetStatusUpdateViewModelAsync(int id)
        {
            try 
            { 
            var shipment = await _shipmentRepo.GetByIdAsync(id);
            if (shipment == null) return null;

            return new ShipmentStatusUpdateViewModel
            {
                Id = shipment.Id,
                ShipmentCode = shipment.ShipmentCode,
                CustomerName = shipment.Customer?.CustomerName ?? "",
                Route = $"{shipment.SourcePort} → {shipment.DestinationPort}",
                CurrentStatus = shipment.Status.ToString(),
                NewStatus = shipment.Status.ToString(),
            };
            }
            catch (Exception ex)
            {
                await _logActionService.LogActionAsync("ShipmentService.GetStatusUpdateViewModelAsync", "Update", "", $"Erorr in this : {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateStatusAsync(ShipmentStatusUpdateViewModel model)
        {
            try
            {
                var shipment = await _shipmentRepo.GetByIdAsync(model.Id);
                if (shipment == null) return false;

                // Parse & update status
                shipment.Status = Enum.Parse<ShipmentStatus>(model.NewStatus);
                shipment.UpdatedAt = DateTime.Now;

                // If delivered, set actual arrival date
                if (shipment.Status == ShipmentStatus.Delivered)
                    shipment.ActualArrivalDate = DateTime.Now;

                await _shipmentRepo.UpdateAsync(shipment);

                // Add status log
                await _shipmentRepo.AddStatusLogAsync(new ShipmentStatusLog
                {
                    ShipmentId = shipment.Id,
                    Status = model.NewStatus,
                    CurrentLocation = model.CurrentLocation,
                    Remarks = model.Remarks,
                    UpdatedDate = DateTime.Now,
                    UpdatedBy = "Admin",
                });

                // Add tracking step
                int nextOrder = shipment.TrackingSteps.Any()
                    ? shipment.TrackingSteps.Max(t => t.StepOrder) + 1 : 2;

                await _shipmentRepo.AddTrackingStepAsync(new ShipmentTracking
                {
                    ShipmentId = shipment.Id,
                    StepTitle = GetStepTitle(model.NewStatus),
                    StepLocation = model.CurrentLocation ?? "",
                    StepDate = DateTime.Now,
                    StepStatus = shipment.Status == ShipmentStatus.Delivered
                                    ? "done" : "current",
                    StepOrder = nextOrder,
                });

                return true;
            }
            catch (Exception ex)
            {
                await _logActionService.LogActionAsync("ShipmentService.UpdateStatusAsync", "Update", "", $"Erorr in this : {ex.Message}");
                return false;
            }
        }

        //  AUTO-DELAY LOGIC 
        public async Task AutoMarkDelayedAsync()
        {
            // Get all shipments past their arrival date
            // that are NOT yet Delivered or already Delayed
            var overdueShipments = await _shipmentRepo.GetOverdueShipmentsAsync();

            foreach (var shipment in overdueShipments)
            {
                // Mark as Delayed
                shipment.Status = ShipmentStatus.Delayed;
                shipment.UpdatedAt = DateTime.Now;
                await _shipmentRepo.UpdateAsync(shipment);

                // Auto log the delay
                await _shipmentRepo.AddStatusLogAsync(new ShipmentStatusLog
                {
                    ShipmentId = shipment.Id,
                    Status = "Delayed",
                    Remarks = $"Auto-marked as Delayed. " +
                                  $"Expected arrival was {shipment.ArrivalDate:dd MMM yyyy}.",
                    UpdatedDate = DateTime.Now,
                    UpdatedBy = "System (Auto)",
                });
            }
        }

        private static string GetStepTitle(string status) => status switch
        {
            "Loaded" => "Cargo Loaded",
            "InTransit" => "Departed Port — In Transit",
            "PortArrival" => "Arrived at Destination Port",
            "CustomsClearance" => "Customs Clearance",
            "Delivered" => "Delivered ✅",
            "Delayed" => "Shipment Delayed ⚠️",
            _ => status,
        };
    }
}