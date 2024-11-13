using AutoMapper;
using LibraryManagementSystem.Core.Interfaces.Repositories;
using LibraryManagementSystem.Core.Models;
using LibraryManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace LibraryManagementSystem.Infrastructure.Repositories
{
    public class LoansRepository : ILoansRepository
    {
        private readonly LibraryDbContext _context;
        private readonly ILogger<LoansRepository> _logger;
        private readonly IMapper _mapper;

        public LoansRepository(LibraryDbContext context, ILogger<LoansRepository> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
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

        public async Task<Loan> AddLoanAsync(Loan loan)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var book = await _context.Books.FindAsync(loan.BookId);
                if (book == null)
                {
                    throw new KeyNotFoundException($"Book with ID {loan.BookId} does not exist.");
                }

                var user = await _context.Users.FindAsync(loan.UserId);
                if (user == null)
                {
                    throw new KeyNotFoundException($"User with ID {loan.UserId} does not exist.");
                }

                await _context.Loans.AddAsync(loan);

                book.IsBorrowed = true;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return loan;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); 
                _logger.LogError(ex, "Error adding a new loan");
                throw; 
            }
        }

        public async Task UpdateLoanAsync(int id, Loan loan)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var existingLoan = await _context.Loans.FindAsync(id);
                if (existingLoan == null)
                {
                    throw new KeyNotFoundException($"Loan with ID {id} does not exist.");
                }


                if ( !existingLoan.IsReturned && loan.IsReturned && loan.ReturnDate.HasValue)
                {
                    var book = await _context.Books.FindAsync(existingLoan.BookId);
                    if (book == null)
                    {
                        throw new KeyNotFoundException($"Book with ID {existingLoan.BookId} does not exist.");
                    }

                    book.IsBorrowed = false;
                }

                _mapper.Map(loan, existingLoan);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
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

        public async Task<IEnumerable<Loan>> GetLoansDueTomorrowAsync()
        {
            try
            {
                return await _context.Loans
                .Where(loan => loan.DueDate.Date == DateTime.Now.AddDays(1).Date)
                .Include(loan => loan.User)
                .Include(loan => loan.Book)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving loans due tomorrow");
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
