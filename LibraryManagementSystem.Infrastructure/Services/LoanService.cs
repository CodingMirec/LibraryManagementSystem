using LibraryManagementSystem.Core.DTOs;
using LibraryManagementSystem.Core.Interfaces.Repositories;
using LibraryManagementSystem.Core.Interfaces.Services;
using LibraryManagementSystem.Core.Models;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace LibraryManagementSystem.Infrastructure.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly ILogger<LoanService> _logger;
        private readonly IMapper _mapper;

        public LoanService(ILoanRepository loanRepository, ILogger<LoanService> logger, IMapper mapper)
        {
            _loanRepository = loanRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LoanDTO>> GetAllLoansAsync()
        {
            try
            {
                var loans = await _loanRepository.GetAllLoansAsync();
                return _mapper.Map<IEnumerable<LoanDTO>>(loans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all loans.");
                throw;
            }
        }

        public async Task<LoanDTO> GetLoanByIdAsync(int id)
        {
            try
            {
                var loan = await _loanRepository.GetLoanByIdAsync(id);
                return _mapper.Map<LoanDTO>(loan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving loan with ID {id}");
                throw;
            }
        }

        public async Task AddLoanAsync(LoanDTO loanDto)
        {
            try
            {
                var loan = _mapper.Map<Loan>(loanDto);
                await _loanRepository.AddLoanAsync(loan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding loan for Book ID {loanDto.BookId} and User ID {loanDto.UserId}");
                throw;
            }
        }

        public async Task UpdateLoanAsync(LoanDTO loanDto)
        {
            try
            {
                var loan = _mapper.Map<Loan>(loanDto);
                await _loanRepository.UpdateLoanAsync(loan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating loan with ID {loanDto.Id}");
                throw;
            }
        }

        public async Task DeleteLoanAsync(int id)
        {
            try
            {
                await _loanRepository.DeleteLoanAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting loan with ID {id}");
                throw;
            }
        }
    }
}