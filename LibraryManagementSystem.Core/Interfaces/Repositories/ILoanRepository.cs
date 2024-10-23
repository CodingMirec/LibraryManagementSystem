using LibraryManagementSystem.Core.Models;

namespace LibraryManagementSystem.Core.Interfaces.Repositories
{
    public interface ILoanRepository
    {
        Task<IEnumerable<Loan>> GetAllLoansAsync();
        Task<Loan> GetLoanByIdAsync(int id);
        Task AddLoanAsync(Loan loan);
        Task UpdateLoanAsync(Loan loan);
        Task DeleteLoanAsync(int id);
        Task<IEnumerable<Loan>> GetLoansByUserIdAsync(int userId);
        Task<IEnumerable<Loan>> GetLoansByBookIdAsync(int bookId);
    }
}
