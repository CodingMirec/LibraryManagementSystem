using LibraryManagementSystem.Core.DTOs;

namespace LibraryManagementSystem.Core.Interfaces.Services
{
    public interface ILoanService
    {
        Task<IEnumerable<LoanDTO>> GetAllLoansAsync();
        Task<LoanDTO> GetLoanByIdAsync(int id);
        Task AddLoanAsync(LoanDTO loanDto);
        Task UpdateLoanAsync(LoanDTO loanDto);
        Task DeleteLoanAsync(int id);
    }
}
