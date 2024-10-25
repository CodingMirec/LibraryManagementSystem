using LibraryManagementSystem.Core.Interfaces.Repositories;
using LibraryManagementSystem.Core.Interfaces.Services;
using LibraryManagementSystem.Core.Models;
using Microsoft.Extensions.Logging;
using AutoMapper;
using LibraryManagementSystem.Core.DTOs.RequestDtos;
using LibraryManagementSystem.Core.DTOs.ResponseDtos;

namespace LibraryManagementSystem.Infrastructure.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _userRepository;
        private readonly ILogger<UsersService> _logger;
        private readonly IMapper _mapper;

        public UsersService(IUsersRepository userRepository, ILogger<UsersService> logger, IMapper mapper)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetAllUsersAsync();
                return _mapper.Map<IEnumerable<UserResponseDto>>(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all users.");
                throw;
            }
        }

        public async Task<UserResponseDto> GetUserByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogError("Invalid ID provided for GetUserByIdAsync.");
                throw new ArgumentException("ID must be greater than zero.", nameof(id));
            }

            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                return _mapper.Map<UserResponseDto>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving user with ID {id}");
                throw;
            }
        }

        public async Task<UserResponseDto> AddUserAsync(UserRequestDto userDto)
        {
            if (userDto == null)
            {
                _logger.LogError("UserDTO is null.");
                throw new ArgumentNullException(nameof(userDto), "UserDTO cannot be null.");
            }

            try
            {
                var user = _mapper.Map<User>(userDto);
                await _userRepository.AddUserAsync(user);

                var userResponseDto = _mapper.Map<UserResponseDto>(user);
                return userResponseDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding user {userDto.FirstName} {userDto.LastName}");
                throw;
            }
        }

        public async Task UpdateUserAsync(int id, UserRequestDto userDto)
        {
            if (userDto == null)
            {
                _logger.LogError("UserDTO is null.");
                throw new ArgumentNullException(nameof(userDto), "UserDTO cannot be null.");
            }

            if (id <= 0)
            {
                _logger.LogError("Invalid ID provided for UpdateUserAsync.");
                throw new ArgumentException("ID must be greater than zero.", nameof(id));
            }

            try
            {
                var user = _mapper.Map<User>(userDto);
                await _userRepository.UpdateUserAsync(id, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating user with ID {id}");
                throw;
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogError("Invalid ID provided for DeleteUserAsync.");
                throw new ArgumentException("ID must be greater than zero.", nameof(id));
            }

            try
            {
                await _userRepository.DeleteUserAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting user with ID {id}");
                throw;
            }
        }
    }
}
