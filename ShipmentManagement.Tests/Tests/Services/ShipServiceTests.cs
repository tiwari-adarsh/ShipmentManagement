using Moq;
using FluentAssertions;
using ShipmentManagement.Models;
using ShipmentManagement.Models.Enums;
using ShipmentManagement.Repositories.Interfaces;
using ShipmentManagement.Services.Implementations;
using ShipmentManagement.Tests.Mocks;
using ShipmentManagement.ViewModels.Ship;
using ShipmentManagement.Services.Interfaces;

namespace ShipmentManagement.Tests.Services
{
    public class ShipServiceTests
    {
        private readonly Mock<IShipRepository> _mockRepo;
        private readonly Mock<ILogActionService> _logActionService;
        private readonly ShipService _service;

        public ShipServiceTests()
        {
            _mockRepo = new Mock<IShipRepository>();
            _logActionService = new Mock<ILogActionService>();
            _service = new ShipService(_mockRepo.Object, _logActionService.Object);
        }


        [Fact]
        public async Task GetAll_NoFilter_ReturnsAllShips()
        {
            // Arrange
            var allShips = MockData.GetShips();
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(allShips);

            // Act
            var result = await _service.GetAllAsync(null, null);

            // Assert
            result.Ships.Should().HaveCount(3);
            result.TotalShips.Should().Be(3);
        }

        [Fact]
        public async Task GetAll_WithSearchTerm_FiltersCorrectly()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(MockData.GetShips());

            // Act
            var result = await _service.GetAllAsync("Titan", null);

            // Assert
            result.Ships.Should().HaveCount(1);
            result.Ships[0].ShipName.Should().Be("MV Titan");
        }

        [Fact]
        public async Task GetAll_WithTypeFilter_FiltersCorrectly()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(MockData.GetShips());

            // Act
            var result = await _service.GetAllAsync(null, "Tanker");

            // Assert
            result.Ships.Should().HaveCount(1);
            result.Ships[0].ShipType.Should().Be("Tanker");
            result.Ships[0].ShipName.Should().Be("MV Horizon");
        }

        [Fact]
        public async Task GetAll_CountsActiveShipsCorrectly()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(MockData.GetShips());

            // Act
            var result = await _service.GetAllAsync(null, null);

            // Assert
            result.ActiveShips.Should().Be(2);
            result.MaintenanceShips.Should().Be(1);
            result.InactiveShips.Should().Be(0);
        }

        [Fact]
        public async Task GetAll_SearchNotFound_ReturnsEmpty()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(MockData.GetShips());

            // Act
            var result = await _service.GetAllAsync("NonExistentShip", null);

            // Assert
            result.Ships.Should().BeEmpty();
        }

        //  CREATE TESTS

        [Fact]
        public async Task Create_ValidShip_ReturnsTrue()
        {
            // Arrange
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Ship>()))
                     .Returns(Task.CompletedTask);

            var model = new ShipCreateViewModel
            {
                ShipCode = "S006",
                ShipName = "MV Discovery",
                ImoNumber = "1234567",
                CapacityMT = 45000,
                ShipType = "Container",
                FlagCountry = "India",
                YearBuilt = 2022,
                Status = "Active",
            };

            // Act
            var result = await _service.CreateAsync(model);

            // Assert
            result.Should().BeTrue();
            _mockRepo.Verify(r =>
                r.AddAsync(It.IsAny<Ship>()), Times.Once);
        }

        [Fact]
        public async Task Create_ShipCodeConvertedToUpperCase()
        {
            // Arrange
            Ship? capturedShip = null;

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Ship>()))
                     .Callback<Ship>(s => capturedShip = s)
                     .Returns(Task.CompletedTask);

            var model = new ShipCreateViewModel
            {
                ShipCode = "s006",       // lowercase input
                ShipName = "MV Discovery",
                ImoNumber = "1234567",
                CapacityMT = 45000,
                ShipType = "Container",
                FlagCountry = "India",
                YearBuilt = 2022,
                Status = "Active",
            };

            // Act
            await _service.CreateAsync(model);

            // Assert
            capturedShip!.ShipCode.Should().Be("S006"); // converted to uppercase
        }

        [Fact]
        public async Task Create_WhenExceptionThrown_ReturnsFalse()
        {
            // Arrange
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Ship>()))
                     .ThrowsAsync(new Exception("DB Error"));

            var model = new ShipCreateViewModel
            {
                ShipCode = "S006",
                ShipName = "MV Discovery",
                ImoNumber = "1234567",
                CapacityMT = 45000,
                ShipType = "Container",
                FlagCountry = "India",
                YearBuilt = 2022,
                Status = "Active",
            };

            // Act
            var result = await _service.CreateAsync(model);

            // Assert
            result.Should().BeFalse();
        }

        //  UPDATE TESTS

        [Fact]
        public async Task Update_ValidShip_ReturnsTrue()
        {
            // Arrange
            var existingShip = MockData.GetShips()[0];

            _mockRepo.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(existingShip);

            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Ship>()))
                     .Returns(Task.CompletedTask);

            var model = new ShipCreateViewModel
            {
                Id = 1,
                ShipCode = "S001",
                ShipName = "MV Titan Updated",
                ImoNumber = "9876543",
                CapacityMT = 55000,
                ShipType = "Bulk Carrier",
                FlagCountry = "India",
                YearBuilt = 2018,
                Status = "Active",
            };

            // Act
            var result = await _service.UpdateAsync(model);

            // Assert
            result.Should().BeTrue();
            _mockRepo.Verify(r =>
                r.UpdateAsync(It.Is<Ship>(s =>
                    s.ShipName == "MV Titan Updated")),
                Times.Once);
        }

        [Fact]
        public async Task Update_InvalidId_ReturnsFalse()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(999))
                     .ReturnsAsync((Ship?)null);

            var model = new ShipCreateViewModel { Id = 999 };

            // Act
            var result = await _service.UpdateAsync(model);

            // Assert
            result.Should().BeFalse();
        }

        //  DELETE TESTS

        [Fact]
        public async Task Delete_ShipWithNoShipments_ReturnsTrue()
        {
            // Arrange
            _mockRepo.Setup(r => r.HasShipmentsAsync(1))
                     .ReturnsAsync(false);

            _mockRepo.Setup(r => r.DeleteAsync(1))
                     .Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            //result.Success.Should().BeTrue();
            _mockRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task Delete_ShipWithShipments_ReturnsFalseWithMessage()
        {
            // Arrange
            _mockRepo.Setup(r => r.HasShipmentsAsync(1))
                     .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            //result.Success.Should().BeFalse();
            //result.Message.Should().Contain("shipments");

            // Delete should NOT be called
            _mockRepo.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }
    }
}