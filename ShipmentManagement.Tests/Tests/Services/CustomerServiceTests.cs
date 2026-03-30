using Moq;
using FluentAssertions;
using ShipmentManagement.Models;
using ShipmentManagement.Repositories.Interfaces;
using ShipmentManagement.Services.Implementations;
using ShipmentManagement.Tests.Mocks;
using ShipmentManagement.ViewModels.Customer;
using ShipmentManagement.Services.Interfaces;

namespace ShipmentManagement.Tests.Services
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _mockRepo;
        private readonly Mock<ILogActionService> _logActionService;
        private readonly CustomerService _service;

        public CustomerServiceTests()
        {
            _mockRepo = new Mock<ICustomerRepository>();
            _logActionService = new Mock<ILogActionService>();
            _service = new CustomerService(_mockRepo.Object,_logActionService.Object);
        }

        //  GET ALL TESTS

        [Fact]
        public async Task GetAll_NoSearch_ReturnsAllCustomers()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(MockData.GetCustomers());

            // Act
            var result = await _service.GetAllAsync(null);

            // Assert
            result.Customers.Should().HaveCount(2);
            result.TotalCustomers.Should().Be(2);
        }

        [Fact]
        public async Task GetAll_WithSearch_FiltersByName()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(MockData.GetCustomers());

            // Act
            var result = await _service.GetAllAsync("Rajesh");

            // Assert
            result.Customers.Should().HaveCount(1);
            result.Customers[0].CustomerName.Should().Be("Rajesh Mehta");
        }

        [Fact]
        public async Task GetAll_WithSearch_FiltersByCompany()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(MockData.GetCustomers());

            // Act
            var result = await _service.GetAllAsync("Tata");

            // Assert
            result.Customers.Should().HaveCount(1);
            result.Customers[0].CompanyName.Should().Be("Tata Steel");
        }

        [Fact]
        public async Task GetAll_WithSearch_FiltersByEmail()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(MockData.GetCustomers());

            // Act
            var result = await _service.GetAllAsync("reliance");

            // Assert
            result.Customers.Should().HaveCount(1);
            result.Customers[0].Email.Should().Contain("reliance");
        }

        [Fact]
        public async Task GetAll_SearchNotFound_ReturnsEmpty()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(MockData.GetCustomers());

            // Act
            var result = await _service.GetAllAsync("NonExistentCustomer");

            // Assert
            result.Customers.Should().BeEmpty();
            result.TotalCustomers.Should().Be(0);
        }

        //  CREATE TESTS

        [Fact]
        public async Task Create_ValidCustomer_ReturnsTrue()
        {
            // Arrange
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Customer>()))
                     .Returns(Task.CompletedTask);

            var model = new CustomerCreateViewModel
            {
                CustomerCode = "C003",
                CustomerName = "Vikram Nair",
                CompanyName = "ONGC",
                Email = "v.nair@ongc.in",
                Phone = "+91 76543 21098",
                City = "Dehradun",
            };

            // Act
            var result = await _service.CreateAsync(model);

            // Assert
            result.Should().BeTrue();
            _mockRepo.Verify(r =>
                r.AddAsync(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async Task Create_CustomerCodeConvertedToUpperCase()
        {
            // Arrange
            Customer? capturedCustomer = null;

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Customer>()))
                     .Callback<Customer>(c => capturedCustomer = c)
                     .Returns(Task.CompletedTask);

            var model = new CustomerCreateViewModel
            {
                CustomerCode = "c003",  // lowercase input
                CustomerName = "Vikram Nair",
                CompanyName = "ONGC",
                Email = "v.nair@ongc.in",
                Phone = "+91 76543 21098",
            };

            // Act
            await _service.CreateAsync(model);

            // Assert
            capturedCustomer!.CustomerCode.Should().Be("C003");
        }

        [Fact]
        public async Task Create_WhenExceptionThrown_ReturnsFalse()
        {
            // Arrange
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Customer>()))
                     .ThrowsAsync(new Exception("DB Error"));

            var model = new CustomerCreateViewModel
            {
                CustomerCode = "C003",
                CustomerName = "Vikram Nair",
                CompanyName = "ONGC",
                Email = "v.nair@ongc.in",
                Phone = "+91 76543 21098",
            };

            // Act
            var result = await _service.CreateAsync(model);

            // Assert
            result.Should().BeFalse();
        }

        //  UPDATE TESTS

        [Fact]
        public async Task Update_ValidCustomer_ReturnsTrue()
        {
            // Arrange
            var existing = MockData.GetCustomers()[0];

            _mockRepo.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(existing);

            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Customer>()))
                     .Returns(Task.CompletedTask);

            var model = new CustomerCreateViewModel
            {
                Id = 1,
                CustomerCode = "C001",
                CustomerName = "Rajesh Mehta Updated",
                CompanyName = "Reliance Industries",
                Phone = "+91 98765 43210",
            };

            // Act
            var result = await _service.UpdateAsync(model);

            // Assert
            result.Should().BeTrue();
            _mockRepo.Verify(r =>
                r.UpdateAsync(It.Is<Customer>(c =>
                    c.CustomerName == "Rajesh Mehta Updated")),
                Times.Once);
        }

        [Fact]
        public async Task Update_EmailNotChanged_OnEdit()
        {
            // Arrange
            var existing = MockData.GetCustomers()[0];
            var originalEmail = existing.Email; // "r.mehta@reliance.com"

            Customer? capturedCustomer = null;

            _mockRepo.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(existing);

            // ── Only ONE Setup for UpdateAsync ──
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Customer>()))
                     .Callback<Customer>(c => capturedCustomer = c)
                     .Returns(Task.CompletedTask);

            var model = new CustomerCreateViewModel
            {
                Id = 1,
                CustomerCode = "C001",
                CustomerName = "Rajesh Mehta",
                CompanyName = "Reliance Industries",
                Phone = "+91 98765 43210",
                Email = "newemail@test.com", // Try to change email
            };

            // Act
            await _service.UpdateAsync(model);

            // Assert — email must stay as original
            capturedCustomer.Should().NotBeNull();
            capturedCustomer!.Email.Should().Be(originalEmail);
        }

        [Fact]
        public async Task Update_InvalidId_ReturnsFalse()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(999))
                     .ReturnsAsync((Customer?)null);

            var model = new CustomerCreateViewModel { Id = 999 };

            // Act
            var result = await _service.UpdateAsync(model);

            // Assert
            result.Should().BeFalse();
        }

        //  DELETE TESTS

        [Fact]
        public async Task Delete_ValidId_ReturnsTrue()
        {
            // Arrange
            _mockRepo.Setup(r => r.DeleteAsync(1))
                     .Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            result.Should().BeTrue();
            _mockRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenExceptionThrown_ReturnsFalse()
        {
            // Arrange
            _mockRepo.Setup(r => r.DeleteAsync(1))
                     .ThrowsAsync(new Exception("FK Constraint"));

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            result.Should().BeFalse();
        }
    }
}