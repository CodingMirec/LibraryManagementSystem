using LibraryManagementSystem.Core.DTOs;
using LibraryManagementSystem.Core.Interfaces.Repositories;
using LibraryManagementSystem.Core.Interfaces.Services;
using LibraryManagementSystem.Core.Models;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _loanRepository;

    public LoanService(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    public async Task<IEnumerable<LoanDTO>> GetAllLoansAsync()
    {
        var loans = await _loanRepository.GetAllLoansAsync();
        var loanDtos = new List<LoanDTO>();

        foreach (var loan in loans)
        {
            loanDtos.Add(new LoanDTO
            {
                Id = loan.Id,
                BookId = loan.BookId,
                UserId = loan.UserId,
                LoanDate = loan.LoanDate,
                DueDate = loan.DueDate,
                ReturnDate = loan.ReturnDate
            });
        }

        return loanDtos;
    }

    public async Task<LoanDTO> GetLoanByIdAsync(int id)
    {
        var loan = await _loanRepository.GetLoanByIdAsync(id);
        return new LoanDTO
        {
            Id = loan.Id,
            BookId = loan.BookId,
            UserId = loan.UserId,
            LoanDate = loan.LoanDate,
            DueDate = loan.DueDate,
            ReturnDate = loan.ReturnDate
        };
    }

    public async Task AddLoanAsync(LoanDTO loanDto)
    {
        var loan = new Loan
        {
            BookId = loanDto.BookId,
            UserId = loanDto.UserId,
            LoanDate = loanDto.LoanDate,
            DueDate = loanDto.DueDate
        };
        await _loanRepository.AddLoanAsync(loan);
    }

    public async Task UpdateLoanAsync(LoanDTO loanDto)
    {
        var loan = new Loan
        {
            Id = loanDto.Id,
            BookId = loanDto.BookId,
            UserId = loanDto.UserId,
            LoanDate = loanDto.LoanDate,
            DueDate = loanDto.DueDate,
            ReturnDate = loanDto.ReturnDate
        };
        await _loanRepository.UpdateLoanAsync(loan);
    }

    public async Task DeleteLoanAsync(int id)
    {
        await _loanRepository.DeleteLoanAsync(id);
    }
}
