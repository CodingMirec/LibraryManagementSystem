using LibraryManagementSystem.Core.Models;

namespace LibraryManagementSystem.Core.Interfaces.Repositories
{
    public interface ILoansRepository
    {
        Task<IEnumerable<Loan>> GetAllLoansAsync();
        Task<Loan> GetLoanByIdAsync(int id);
        Task<Loan> AddLoanAsync(Loan loan);
        Task UpdateLoanAsync(int id, Loan loan);
        Task DeleteLoanAsync(int id);
        Task<IEnumerable<Loan>> GetLoansByUserIdAsync(int userId);
        Task<IEnumerable<Loan>> GetLoansByBookIdAsync(int bookId);
    }
}
