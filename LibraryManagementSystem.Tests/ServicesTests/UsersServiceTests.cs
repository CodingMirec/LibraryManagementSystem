using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using LibraryManagementSystem.Core.DTOs.RequestDtos;
using LibraryManagementSystem.Core.DTOs.ResponseDtos;
using LibraryManagementSystem.Core.Interfaces.Repositories;
using LibraryManagementSystem.Core.Models;
using LibraryManagementSystem.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace LibraryManagementSystem.UnitTests.ServicesTests
{
    public class UsersServiceTests
    {
        private readonly Mock<IUsersRepository> _mockUserRepository;
        private readonly Mock<ILogger<UsersService>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper; 
        private readonly UsersService _userService;

        public UsersServiceTests()
        {
            _mockUserRepository = new Mock<IUsersRepository>();
            _mockLogger = new Mock<ILogger<UsersService>>();
            _mockMapper = new Mock<IMapper>(); 

            _userService = new UsersService(_mockUserRepository.Object, _mockLogger.Object, _mockMapper.Object); 
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnUserDTOs()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, FirstName = "John", LastName = "Doe" },
                new User { Id = 2, FirstName = "Jane", LastName = "Doe" }
            };

            var userDtos = new List<UserResponseDto>
            {
                new UserResponseDto { FirstName = "John", LastName = "Doe" },
                new UserResponseDto { FirstName = "Jane", LastName = "Doe" }
            };

            _mockUserRepository.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(users);
            _mockMapper.Setup(m => m.Map<IEnumerable<UserResponseDto>>(It.IsAny<IEnumerable<User>>())).Returns(userDtos); 

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().ContainSingle(u => u.FirstName == "John" && u.LastName == "Doe");
            result.Should().ContainSingle(u => u.FirstName == "Jane" && u.LastName == "Doe");
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUserDTO_WhenIdIsValid()
        {
            // Arrange
            var user = new User { Id = 1, FirstName = "John", LastName = "Doe" };
            var userDto = new UserResponseDto { FirstName = "John", LastName = "Doe" };

            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(1)).ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<UserResponseDto>(user)).Returns(userDto); 

            // Act
            var result = await _userService.GetUserByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.FirstName.Should().Be("John");
            result.LastName.Should().Be("Doe");
        }

        [Fact]
        public async Task AddUserAsync_ShouldAddUser_WhenUserDtoIsValid()
        {
            // Arrange
            var userDto = new UserRequestDto { FirstName = "John", LastName = "Doe" };
            var user = new User { Id = 1, FirstName = "John", LastName = "Doe" };

            _mockUserRepository.Setup(repo => repo.AddUserAsync(user)).ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<User>(userDto)).Returns(user); 

            // Act
            await _userService.AddUserAsync(userDto);

            // Assert
            _mockUserRepository.Verify(repo => repo.AddUserAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldUpdateUser_WhenUserDtoIsValid()
        {
            // Arrange
            var id = 1;
            var userDto = new UserRequestDto { FirstName = "John", LastName = "Doe" };
            var user = new User { Id = 1, FirstName = "John", LastName = "Doe" };

            _mockUserRepository.Setup(repo => repo.UpdateUserAsync(id, user)).Returns(Task.CompletedTask);
            _mockMapper.Setup(m => m.Map<User>(userDto)).Returns(user); 

            // Act
            await _userService.UpdateUserAsync(id, userDto);

            // Assert
            _mockUserRepository.Verify(repo => repo.UpdateUserAsync(It.IsAny<int>(), It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldDeleteUser_WhenIdIsValid()
        {
            // Arrange
            var userId = 1;
            _mockUserRepository.Setup(repo => repo.DeleteUserAsync(userId)).Returns(Task.CompletedTask);

            // Act
            await _userService.DeleteUserAsync(userId);

            // Assert
            _mockUserRepository.Verify(repo => repo.DeleteUserAsync(userId), Times.Once);
        }
    }
}
