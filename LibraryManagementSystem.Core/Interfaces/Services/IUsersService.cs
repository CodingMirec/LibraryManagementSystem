using LibraryManagementSystem.Core.DTOs.RequestDtos;
using LibraryManagementSystem.Core.DTOs.ResponseDtos;

namespace LibraryManagementSystem.Core.Interfaces.Services
{
    public interface IUsersService
    {
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<UserResponseDto> GetUserByIdAsync(int id);
        Task<UserResponseDto> AddUserAsync(UserRequestDto userDto);
        Task UpdateUserAsync(int id,UserRequestDto userDto);
        Task DeleteUserAsync(int id);
    }
}
