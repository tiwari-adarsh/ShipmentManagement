using FluentAssertions;
using Moq;
using ShipmentManagement.Models;
using ShipmentManagement.Models.Enums;
using ShipmentManagement.Repositories.Interfaces;
using ShipmentManagement.Services.Implementations;
using ShipmentManagement.Services.Interfaces;
using ShipmentManagement.Tests.Mocks;
using ShipmentManagement.ViewModels.Shipment;

namespace ShipmentManagement.Tests.Services
{
    public class ShipmentServiceTests
    {
        // Mocks  
        private readonly Mock<IShipmentRepository> _mockShipmentRepo;
        private readonly Mock<IShipRepository> _mockShipRepo;
        private readonly Mock<ICustomerRepository> _mockCustomerRepo;
        private readonly Mock<IPortRepository> _mockPortRepo;
        private readonly Mock<ILogActionService> _logActionService;
        private readonly ShipmentService _service;

        public ShipmentServiceTests()
        {
            _mockShipmentRepo = new Mock<IShipmentRepository>();
            _mockShipRepo = new Mock<IShipRepository>();
            _mockCustomerRepo = new Mock<ICustomerRepository>();
            _mockPortRepo = new Mock<IPortRepository>();
            _logActionService = new Mock<ILogActionService>();

            _service = new ShipmentService(
                _mockShipmentRepo.Object,
                _mockShipRepo.Object,
                _mockCustomerRepo.Object,
                _mockPortRepo.Object,
                _logActionService.Object
            );
        }

        //  CREATE SHIPMENT TESTS

        [Fact]
        public async Task CreateShipment_ValidData_ReturnsTrue()
        {
            // Arrange
            _mockShipmentRepo
                .Setup(r => r.GenerateShipmentCodeAsync())
                .ReturnsAsync("SHP-2024-0004");

            _mockShipmentRepo
                .Setup(r => r.AddAsync(It.IsAny<Shipment>()))
                .Returns(Task.CompletedTask);

            _mockShipmentRepo
                .Setup(r => r.AddTrackingStepAsync(It.IsAny<ShipmentTracking>()))
                .Returns(Task.CompletedTask);

            _mockShipmentRepo
                .Setup(r => r.AddStatusLogAsync(It.IsAny<ShipmentStatusLog>()))
                .Returns(Task.CompletedTask);

            var model = new ShipmentCreateViewModel
            {
                BookingDate = DateTime.Today,
                ShipmentType = "Export",
                CargoType = "GeneralCargo",
                WeightMT = 2500,
                ShipId = 1,
                CustomerId = 1,
                SourcePort = "Mumbai Port",
                DestinationPort = "Jebel Ali Dubai",
                DepartureDate = DateTime.Today.AddDays(2),
                ArrivalDate = DateTime.Today.AddDays(10),
            };

            // Act
            var result = await _service.CreateShipmentAsync(model);

            // Assert
            result.Should().BeTrue();
            _mockShipmentRepo.Verify(r => r.AddAsync(
                It.IsAny<Shipment>()), Times.Once);
        }

        [Fact]
        public async Task CreateShipment_SetsStatusToPending()
        {
            // Arrange
            Shipment? capturedShipment = null;

            _mockShipmentRepo
                .Setup(r => r.GenerateShipmentCodeAsync())
                .ReturnsAsync("SHP-2024-0005");

            _mockShipmentRepo
                .Setup(r => r.AddAsync(It.IsAny<Shipment>()))
                .Callback<Shipment>(s => capturedShipment = s)
                .Returns(Task.CompletedTask);

            _mockShipmentRepo
                .Setup(r => r.AddTrackingStepAsync(It.IsAny<ShipmentTracking>()))
                .Returns(Task.CompletedTask);

            _mockShipmentRepo
                .Setup(r => r.AddStatusLogAsync(It.IsAny<ShipmentStatusLog>()))
                .Returns(Task.CompletedTask);

            var model = new ShipmentCreateViewModel
            {
                BookingDate = DateTime.Today,
                ShipmentType = "Export",
                CargoType = "GeneralCargo",
                WeightMT = 1000,
                ShipId = 1,
                CustomerId = 1,
                SourcePort = "Mumbai Port",
                DestinationPort = "Singapore PSA",
                DepartureDate = DateTime.Today.AddDays(1),
            };

            // Act
            await _service.CreateShipmentAsync(model);

            // Assert
            capturedShipment.Should().NotBeNull();
            capturedShipment!.Status.Should().Be(ShipmentStatus.Pending);
        }

        [Fact]
        public async Task CreateShipment_AutoGeneratesShipmentCode()
        {
            // Arrange
            Shipment? capturedShipment = null;
            var expectedCode = "SHP-2024-0010";

            _mockShipmentRepo
                .Setup(r => r.GenerateShipmentCodeAsync())
                .ReturnsAsync(expectedCode);

            _mockShipmentRepo
                .Setup(r => r.AddAsync(It.IsAny<Shipment>()))
                .Callback<Shipment>(s => capturedShipment = s)
                .Returns(Task.CompletedTask);

            _mockShipmentRepo
                .Setup(r => r.AddTrackingStepAsync(It.IsAny<ShipmentTracking>()))
                .Returns(Task.CompletedTask);

            _mockShipmentRepo
                .Setup(r => r.AddStatusLogAsync(It.IsAny<ShipmentStatusLog>()))
                .Returns(Task.CompletedTask);

            var model = new ShipmentCreateViewModel
            {
                BookingDate = DateTime.Today,
                ShipmentType = "Import",
                CargoType = "BulkCargo",
                WeightMT = 5000,
                ShipId = 2,
                CustomerId = 2,
                SourcePort = "Jebel Ali Dubai",
                DestinationPort = "Mumbai Port",
                DepartureDate = DateTime.Today.AddDays(3),
            };

            // Act
            await _service.CreateShipmentAsync(model);

            // Assert
            capturedShipment!.ShipmentCode.Should().Be(expectedCode);
        }

        [Fact]
        public async Task CreateShipment_AddsInitialTrackingStep()
        {
            // Arrange
            _mockShipmentRepo
                .Setup(r => r.GenerateShipmentCodeAsync())
                .ReturnsAsync("SHP-2024-0006");

            _mockShipmentRepo
                .Setup(r => r.AddAsync(It.IsAny<Shipment>()))
                .Returns(Task.CompletedTask);

            _mockShipmentRepo
                .Setup(r => r.AddTrackingStepAsync(It.IsAny<ShipmentTracking>()))
                .Returns(Task.CompletedTask);

            _mockShipmentRepo
                .Setup(r => r.AddStatusLogAsync(It.IsAny<ShipmentStatusLog>()))
                .Returns(Task.CompletedTask);

            var model = new ShipmentCreateViewModel
            {
                BookingDate = DateTime.Today,
                ShipmentType = "Export",
                CargoType = "Container",
                WeightMT = 3000,
                ShipId = 1,
                CustomerId = 1,
                SourcePort = "Mumbai Port",
                DestinationPort = "Singapore PSA",
                DepartureDate = DateTime.Today.AddDays(2),
            };

            // Act
            await _service.CreateShipmentAsync(model);

            // Assert — tracking step and status log both added
            _mockShipmentRepo.Verify(r =>
                r.AddTrackingStepAsync(It.IsAny<ShipmentTracking>()),
                Times.Once);

            _mockShipmentRepo.Verify(r =>
                r.AddStatusLogAsync(It.IsAny<ShipmentStatusLog>()),
                Times.Once);
        }

        [Fact]
        public async Task CreateShipment_WhenExceptionThrown_ReturnsFalse()
        {
            // Arrange
            _mockShipmentRepo
                .Setup(r => r.GenerateShipmentCodeAsync())
                .ThrowsAsync(new Exception("DB Error"));

            var model = new ShipmentCreateViewModel
            {
                BookingDate = DateTime.Today,
                ShipmentType = "Export",
                CargoType = "GeneralCargo",
                WeightMT = 1000,
                ShipId = 1,
                CustomerId = 1,
                SourcePort = "Mumbai Port",
                DestinationPort = "Dubai",
                DepartureDate = DateTime.Today.AddDays(1),
            };

            // Act
            var result = await _service.CreateShipmentAsync(model);

            // Assert
            result.Should().BeFalse();
        }

        //  AUTO-DELAY TESTS

        [Fact]
        public async Task AutoMarkDelayed_OverdueShipment_MarksAsDelayed()
        {
            // Arrange
            var overdueShipments = MockData.GetOverdueShipments();

            _mockShipmentRepo
                .Setup(r => r.GetOverdueShipmentsAsync())
                .ReturnsAsync(overdueShipments);

            _mockShipmentRepo
                .Setup(r => r.UpdateAsync(It.IsAny<Shipment>()))
                .Returns(Task.CompletedTask);

            _mockShipmentRepo
                .Setup(r => r.AddStatusLogAsync(It.IsAny<ShipmentStatusLog>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.AutoMarkDelayedAsync();

            // Assert — UpdateAsync called once per overdue shipment
            _mockShipmentRepo.Verify(r =>
                r.UpdateAsync(It.Is<Shipment>(s =>
                    s.Status == ShipmentStatus.Delayed)),
                Times.Exactly(overdueShipments.Count));
        }

        [Fact]
        public async Task AutoMarkDelayed_NoOverdueShipments_DoesNotUpdate()
        {
            // Arrange — empty list
            _mockShipmentRepo
                .Setup(r => r.GetOverdueShipmentsAsync())
                .ReturnsAsync(new List<Shipment>());

            // Act
            await _service.AutoMarkDelayedAsync();

            // Assert — update never called
            _mockShipmentRepo.Verify(r =>
                r.UpdateAsync(It.IsAny<Shipment>()),
                Times.Never);
        }

        [Fact]
        public async Task AutoMarkDelayed_AddsStatusLog_ForEachOverdueShipment()
        {
            // Arrange
            var overdueShipments = MockData.GetOverdueShipments();

            _mockShipmentRepo
                .Setup(r => r.GetOverdueShipmentsAsync())
                .ReturnsAsync(overdueShipments);

            _mockShipmentRepo
                .Setup(r => r.UpdateAsync(It.IsAny<Shipment>()))
                .Returns(Task.CompletedTask);

            _mockShipmentRepo
                .Setup(r => r.AddStatusLogAsync(It.IsAny<ShipmentStatusLog>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.AutoMarkDelayedAsync();

            // Assert — one log per overdue shipment
            _mockShipmentRepo.Verify(r =>
                r.AddStatusLogAsync(It.IsAny<ShipmentStatusLog>()),
                Times.Exactly(overdueShipments.Count));
        }

        //  STATUS UPDATE TESTS

        [Fact]
        public async Task UpdateStatus_ValidId_UpdatesSuccessfully()
        {
            // Arrange
            var shipment = MockData.GetShipments()[0];

            _mockShipmentRepo
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(shipment);

            _mockShipmentRepo
                .Setup(r => r.UpdateAsync(It.IsAny<Shipment>()))
                .Returns(Task.CompletedTask);

            _mockShipmentRepo
                .Setup(r => r.AddStatusLogAsync(It.IsAny<ShipmentStatusLog>()))
                .Returns(Task.CompletedTask);

            _mockShipmentRepo
                .Setup(r => r.AddTrackingStepAsync(It.IsAny<ShipmentTracking>()))
                .Returns(Task.CompletedTask);

            var model = new ShipmentStatusUpdateViewModel
            {
                Id = 1,
                ShipmentCode = "SHP-2024-0001",
                NewStatus = "PortArrival",
                CurrentLocation = "Jebel Ali Port",
                Remarks = "Arrived at destination",
            };

            // Act
            var result = await _service.UpdateStatusAsync(model);

            // Assert
            result.Should().BeTrue();
            _mockShipmentRepo.Verify(r =>
                r.UpdateAsync(It.Is<Shipment>(s =>
                    s.Status == ShipmentStatus.PortArrival)),
                Times.Once);
        }

        [Fact]
        public async Task UpdateStatus_WhenDelivered_SetsActualArrivalDate()
        {
            // Arrange
            var shipment = MockData.GetShipments()[0];
            shipment.ActualArrivalDate = null;

            _mockShipmentRepo
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(shipment);

            _mockShipmentRepo
                .Setup(r => r.UpdateAsync(It.IsAny<Shipment>()))
                .Returns(Task.CompletedTask);

            _mockShipmentRepo
                .Setup(r => r.AddStatusLogAsync(It.IsAny<ShipmentStatusLog>()))
                .Returns(Task.CompletedTask);

            _mockShipmentRepo
                .Setup(r => r.AddTrackingStepAsync(It.IsAny<ShipmentTracking>()))
                .Returns(Task.CompletedTask);

            var model = new ShipmentStatusUpdateViewModel
            {
                Id = 1,
                NewStatus = "Delivered",
            };

            // Act
            await _service.UpdateStatusAsync(model);

            // Assert
            shipment.ActualArrivalDate.Should().NotBeNull();
            shipment.ActualArrivalDate.Should().BeCloseTo(
                DateTime.Now, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task UpdateStatus_InvalidId_ReturnsFalse()
        {
            // Arrange — return null for unknown ID
            _mockShipmentRepo
                .Setup(r => r.GetByIdAsync(999))
                .ReturnsAsync((Shipment?)null);

            var model = new ShipmentStatusUpdateViewModel
            {
                Id = 999,
                NewStatus = "Delivered",
            };

            // Act
            var result = await _service.UpdateStatusAsync(model);

            // Assert
            result.Should().BeFalse();
        }

        //  GET ALL / LISTING TESTS

        [Fact]
        public async Task GetAll_ReturnsAllShipments()
        {
            // Arrange
            var shipments = MockData.GetShipments();

            _mockShipmentRepo
                .Setup(r => r.GetFilteredAsync(null, null, null))
                .ReturnsAsync(shipments);

            _mockShipmentRepo
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(shipments);

            _mockShipmentRepo
                .Setup(r => r.GetOverdueShipmentsAsync())
                .ReturnsAsync(new List<Shipment>());

            _mockShipmentRepo
                .Setup(r => r.UpdateAsync(It.IsAny<Shipment>()))
                .Returns(Task.CompletedTask);

            _mockShipmentRepo
                .Setup(r => r.AddStatusLogAsync(It.IsAny<ShipmentStatusLog>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.GetAllAsync(null, null, null);

            // Assert
            result.Should().NotBeNull();
            result.Shipments.Should().HaveCount(shipments.Count);
            result.TotalCount.Should().Be(shipments.Count);
        }

        [Fact]
        public async Task GetAll_WithSearchTerm_PassesSearchToRepo()
        {
            // Arrange
            var shipments = MockData.GetShipments()
                .Where(s => s.ShipmentCode.Contains("0001"))
                .ToList();

            _mockShipmentRepo
                .Setup(r => r.GetFilteredAsync("0001", null, null))
                .ReturnsAsync(shipments);

            _mockShipmentRepo
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(MockData.GetShipments());

            _mockShipmentRepo
                .Setup(r => r.GetOverdueShipmentsAsync())
                .ReturnsAsync(new List<Shipment>());

            _mockShipmentRepo
                .Setup(r => r.UpdateAsync(It.IsAny<Shipment>()))
                .Returns(Task.CompletedTask);

            _mockShipmentRepo
                .Setup(r => r.AddStatusLogAsync(It.IsAny<ShipmentStatusLog>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.GetAllAsync("0001", null, null);

            // Assert
            result.Shipments.Should().HaveCount(1);
            result.SearchTerm.Should().Be("0001");
        }

        [Fact]
        public async Task GetAll_CountsDeliveredCorrectly()
        {
            // Arrange
            var shipments = MockData.GetShipments();

            _mockShipmentRepo
                .Setup(r => r.GetFilteredAsync(null, null, null))
                .ReturnsAsync(shipments);

            _mockShipmentRepo
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(shipments);

            _mockShipmentRepo
                .Setup(r => r.GetOverdueShipmentsAsync())
                .ReturnsAsync(new List<Shipment>());

            _mockShipmentRepo
                .Setup(r => r.UpdateAsync(It.IsAny<Shipment>()))
                .Returns(Task.CompletedTask);

            _mockShipmentRepo
                .Setup(r => r.AddStatusLogAsync(It.IsAny<ShipmentStatusLog>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.GetAllAsync(null, null, null);

            // Assert
            var expectedDelivered = shipments
                .Count(s => s.Status == ShipmentStatus.Delivered);
            result.DeliveredCount.Should().Be(expectedDelivered);
        }

        //  GET DETAILS TESTS

        [Fact]
        public async Task GetDetails_ValidId_ReturnsViewModel()
        {
            // Arrange
            var shipment = MockData.GetShipments()[0];

            _mockShipmentRepo
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(shipment);

            // Act
            var result = await _service.GetDetailsAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.Shipment.ShipmentCode.Should().Be("SHP-2024-0001");
        }

        [Fact]
        public async Task GetDetails_InvalidId_ReturnsNull()
        {
            // Arrange
            _mockShipmentRepo
                .Setup(r => r.GetByIdAsync(999))
                .ReturnsAsync((Shipment?)null);

            // Act
            var result = await _service.GetDetailsAsync(999);

            // Assert
            result.Should().BeNull();
        }
    }
}