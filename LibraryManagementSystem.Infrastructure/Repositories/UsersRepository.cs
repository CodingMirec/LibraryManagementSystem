using LibraryManagementSystem.Core.Interfaces.Repositories;
using LibraryManagementSystem.Core.Models;
using LibraryManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly LibraryDbContext _context;
        private readonly ILogger<UsersRepository> _logger;

        public UsersRepository(LibraryDbContext context, ILogger<UsersRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all users");
                throw; 
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                return await _context.Users.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving user with ID {id}");
                throw; 
            }
        }

        public async Task<User> AddUserAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding a new user");
                throw; 
            }
        }

        public async Task UpdateUserAsync(int id, User user)
        {
            try
            {
                user.Id = id;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating the user");
                throw; 
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            try
            {
                var user = await GetUserByIdAsync(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _logger.LogWarning($"User with ID {id} not found for deletion");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting user with ID {id}");
                throw; 
            }
        }
    }
}
