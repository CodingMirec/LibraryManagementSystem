using LibraryManagementSystem.Core.DTOs.RequestDtos;
using LibraryManagementSystem.Core.DTOs.ResponseDtos;

namespace LibraryManagementSystem.Core.Interfaces.Services
{
    public interface ILoansService
    {
        Task<IEnumerable<LoanResponseDto>> GetAllLoansAsync();
        Task<LoanResponseDto> GetLoanByIdAsync(int id);
        Task<LoanResponseDto> AddLoanAsync(LoanRequestDto loanDto);
        Task UpdateLoanAsync(int id, LoanRequestDto loanDto);
        Task DeleteLoanAsync(int id);
    }
}
