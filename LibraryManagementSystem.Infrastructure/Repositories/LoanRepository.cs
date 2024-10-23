using LibraryManagementSystem.Core.Interfaces.Repositories;
using LibraryManagementSystem.Core.Models;
using LibraryManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Infrastructure.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly LibraryDbContext _context;
        private readonly ILogger<LoanRepository> _logger;

        public LoanRepository(LibraryDbContext context, ILogger<LoanRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Loan>> GetAllLoansAsync()
        {
            try
            {
                return await _context.Loans.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all loans");
                throw; 
            }
        }

        public async Task<Loan> GetLoanByIdAsync(int id)
        {
            try
            {
                return await _context.Loans.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving loan with ID {id}");
                throw; 
            }
        }

        public async Task AddLoanAsync(Loan loan)
        {
            try
            {
                await _context.Loans.AddAsync(loan);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding a new loan");
                throw; 
            }
        }

        public async Task UpdateLoanAsync(Loan loan)
        {
            try
            {
                _context.Loans.Update(loan);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating the loan");
                throw; 
            }
        }

        public async Task DeleteLoanAsync(int id)
        {
            try
            {
                var loan = await GetLoanByIdAsync(id);
                if (loan != null)
                {
                    _context.Loans.Remove(loan);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _logger.LogWarning($"Loan with ID {id} not found for deletion");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting loan with ID {id}");
                throw; 
            }
        }

        public async Task<IEnumerable<Loan>> GetLoansByUserIdAsync(int userId)
        {
            try
            {
                return await _context.Loans.Where(loan => loan.UserId == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving loans for user with ID {userId}");
                throw; 
            }
        }

        public async Task<IEnumerable<Loan>> GetLoansByBookIdAsync(int bookId)
        {
            try
            {
                return await _context.Loans.Where(loan => loan.BookId == bookId).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving loans for book with ID {bookId}");
                throw; 
            }
        }
    }
}
