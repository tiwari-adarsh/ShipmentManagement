using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using ShipmentManagement.Models;
using ShipmentManagement.Repositories.Interfaces;
using ShipmentManagement.Services.Implementations;
using ShipmentManagement.ViewModels.Account;
using ShipmentManagement.Services.Interfaces;

namespace ShipmentManagement.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly AuthService _service;
        private readonly Mock<ISession> _mockSession;
        private readonly Mock<ILogActionService> _IlogActionservice;

        public AuthServiceTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            _IlogActionservice = new Mock<ILogActionService>();
            _service = new AuthService(_mockRepo.Object,_IlogActionservice.Object);
            _mockSession = new Mock<ISession>();
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsTrue()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "Admin",
                Email = "admin@aquafreight.com",
                PasswordHash = "admin123",
                Role = "Admin",
                IsActive = true,
            };

            _mockRepo
                .Setup(r => r.ValidateUserAsync(
                    "admin@aquafreight.com", "admin123"))
                .ReturnsAsync(user);

            _mockRepo
                .Setup(r => r.UpdateLastLoginAsync(1))
                .Returns(Task.CompletedTask);

            // Setup session to accept SetString
            _mockSession
                .Setup(s => s.Set(It.IsAny<string>(),
                    It.IsAny<byte[]>()));

            var model = new LoginViewModel
            {
                Email = "admin@aquafreight.com",
                Password = "admin123",
            };

            // Act
            var result = await _service.LoginAsync(model, _mockSession.Object);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsFalse()
        {
            // Arrange
            _mockRepo
                .Setup(r => r.ValidateUserAsync(
                    It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            var model = new LoginViewModel
            {
                Email = "wrong@email.com",
                Password = "wrongpass",
            };

            // Act
            var result = await _service.LoginAsync(model, _mockSession.Object);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Login_ValidUser_UpdatesLastLogin()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "Admin",
                Email = "admin@aquafreight.com",
                PasswordHash = "admin123",
                Role = "Admin",
                IsActive = true,
            };

            _mockRepo
                .Setup(r => r.ValidateUserAsync(
                    "admin@aquafreight.com", "admin123"))
                .ReturnsAsync(user);

            _mockRepo
                .Setup(r => r.UpdateLastLoginAsync(1))
                .Returns(Task.CompletedTask);

            _mockSession
                .Setup(s => s.Set(It.IsAny<string>(),
                    It.IsAny<byte[]>()));

            var model = new LoginViewModel
            {
                Email = "admin@aquafreight.com",
                Password = "admin123",
            };

            // Act
            await _service.LoginAsync(model, _mockSession.Object);

            // Assert
            _mockRepo.Verify(r =>
                r.UpdateLastLoginAsync(1), Times.Once);
        }

        [Fact]
        public void IsLoggedIn_WithSessionEmail_ReturnsTrue()
        {
            // Arrange
            var emailBytes = System.Text.Encoding.UTF8
                .GetBytes("admin@aquafreight.com");

            _mockSession
                .Setup(s => s.TryGetValue("UserEmail", out emailBytes!))
                .Returns(true);

            // Act
            var result = _service.IsLoggedIn(_mockSession.Object);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsLoggedIn_WithoutSession_ReturnsFalse()
        {
            // Arrange
            byte[]? nullBytes = null;

            _mockSession
                .Setup(s => s.TryGetValue("UserEmail", out nullBytes!))
                .Returns(false);

            // Act
            var result = _service.IsLoggedIn(_mockSession.Object);

            // Assert
            result.Should().BeFalse();
        }
    }
}