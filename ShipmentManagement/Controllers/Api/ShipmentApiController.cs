using Microsoft.AspNetCore.Mvc;
using ShipmentManagement.DTOs.Shipment;
using ShipmentManagement.Repositories.Interfaces;

namespace ShipmentManagement.Controllers.Api
{
    [ApiController]
    [Route("api/shipments")]
    [Produces("application/json")]
    public class ShipmentApiController : ControllerBase
    {
        private readonly IShipmentRepository _shipmentRepo;

        public ShipmentApiController(IShipmentRepository shipmentRepo)
        {
            _shipmentRepo = shipmentRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var shipments = await _shipmentRepo.GetAllAsync();
            var dtos = shipments.Select(MapToDto).ToList();

            return Ok(new ApiResponseDto<List<ShipmentDto>>
            {
                Success = true,
                Message = "Shipments retrieved successfully",
                Data = dtos,
                Count = dtos.Count,
            });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var shipment = await _shipmentRepo.GetByIdWithDetailsAsync(id);

            if (shipment == null)
                return NotFound(new ApiResponseDto<ShipmentDto>
                {
                    Success = false,
                    Message = $"Shipment with ID {id} not found.",
                    Data = null,
                });

            return Ok(new ApiResponseDto<ShipmentDto>
            {
                Success = true,
                Message = "Shipment retrieved successfully",
                Data = MapToDto(shipment),
                Count = 1,
            });
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus(string status)
        {
            var validStatuses = new[]
            {
                "Pending","Loaded","InTransit",
                "PortArrival","CustomsClearance",
                "Delivered","Delayed"
            };

            if (!validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
                return BadRequest(new ApiResponseDto<List<ShipmentDto>>
                {
                    Success = false,
                    Message = $"Invalid status '{status}'. " +
                              $"Valid: {string.Join(", ", validStatuses)}",
                });

            var shipments = await _shipmentRepo.GetByStatusAsync(status);
            var dtos = shipments.Select(MapToDto).ToList();

            return Ok(new ApiResponseDto<List<ShipmentDto>>
            {
                Success = true,
                Message = $"Shipments with status '{status}' retrieved",
                Data = dtos,
                Count = dtos.Count,
            });
        }

        [HttpGet("code/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var shipment = await _shipmentRepo.GetByCodeAsync(code);

            if (shipment == null)
                return NotFound(new ApiResponseDto<ShipmentDto>
                {
                    Success = false,
                    Message = $"Shipment code '{code}' not found.",
                });

            return Ok(new ApiResponseDto<ShipmentDto>
            {
                Success = true,
                Message = "Shipment retrieved successfully",
                Data = MapToDto(shipment),
                Count = 1,
            });
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var all = await _shipmentRepo.GetAllAsync();

            var stats = new
            {
                Total = all.Count,
                Pending = all.Count(s => s.Status.ToString() == "Pending"),
                InTransit = all.Count(s => s.Status.ToString() == "InTransit"),
                Delivered = all.Count(s => s.Status.ToString() == "Delivered"),
                Delayed = all.Count(s => s.Status.ToString() == "Delayed"),
                PortArrival = all.Count(s => s.Status.ToString() == "PortArrival"),
            };

            return Ok(new ApiResponseDto<object>
            {
                Success = true,
                Message = "Stats retrieved successfully",
                Data = stats,
            });
        }

        private static ShipmentDto MapToDto(Models.Shipment s) => new()
        {
            Id = s.Id,
            ShipmentCode = s.ShipmentCode,
            CustomerName = s.Customer?.CustomerName ?? "",
            CompanyName = s.Customer?.CompanyName ?? "",
            ShipName = s.Ship?.ShipName ?? "",
            ShipCode = s.Ship?.ShipCode ?? "",
            ShipmentType = s.ShipmentType.ToString(),
            CargoType = s.CargoType.ToString(),
            WeightMT = s.WeightMT,
            SourcePort = s.SourcePort,
            DestinationPort = s.DestinationPort,
            BookingDate = s.BookingDate,
            DepartureDate = s.DepartureDate,
            ArrivalDate = s.ArrivalDate,
            ActualArrivalDate = s.ActualArrivalDate,
            Status = s.Status.ToString(),
            ContainerNumber = s.ContainerNumber,
            CreatedAt = s.CreatedAt,
        };
    }
}