using LibraryManagementSystem.Core.Models;

namespace LibraryManagementSystem.Core.Interfaces.Repositories
{
    public interface IUsersRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> AddUserAsync(User user);
        Task UpdateUserAsync(int id, User user);
        Task DeleteUserAsync(int id);
    }

}
