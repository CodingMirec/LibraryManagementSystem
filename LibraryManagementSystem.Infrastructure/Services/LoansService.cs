using LibraryManagementSystem.Core.Interfaces.Repositories;
using LibraryManagementSystem.Core.Interfaces.Services;
using LibraryManagementSystem.Core.Models;
using Microsoft.Extensions.Logging;
using AutoMapper;
using LibraryManagementSystem.Core.DTOs.RequestDtos;
using LibraryManagementSystem.Core.DTOs.ResponseDtos;

namespace LibraryManagementSystem.Infrastructure.Services
{
    public class LoansService : ILoansService
    {
        private readonly ILoansRepository _loanRepository;
        private readonly ILogger<LoansService> _logger;
        private readonly IMapper _mapper;

        public LoansService(ILoansRepository loanRepository, ILogger<LoansService> logger, IMapper mapper)
        {
            _loanRepository = loanRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LoanResponseDto>> GetAllLoansAsync()
        {
            try
            {
                var loans = await _loanRepository.GetAllLoansAsync();
                return _mapper.Map<IEnumerable<LoanResponseDto>>(loans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all loans.");
                throw;
            }
        }

        public async Task<LoanResponseDto> GetLoanByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogError("Invalid ID provided for GetLoanByIdAsync.");
                throw new ArgumentException("ID must be greater than zero.", nameof(id));
            }

            try
            {
                var loan = await _loanRepository.GetLoanByIdAsync(id);
                return _mapper.Map<LoanResponseDto>(loan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving loan with ID {id}");
                throw;
            }
        }

        public async Task<LoanResponseDto> AddLoanAsync(LoanRequestDto loanDto)
        {
            if (loanDto == null)
            {
                _logger.LogError("LoanDTO is null.");
                throw new ArgumentNullException(nameof(loanDto), "LoanDTO cannot be null.");
            }

            try
            {
                var loan = _mapper.Map<Loan>(loanDto);
                await _loanRepository.AddLoanAsync(loan);

                var loanResponseDto = _mapper.Map<LoanResponseDto>(loan);
                return loanResponseDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding loan for Book ID {loanDto.BookId} and User ID {loanDto.UserId}");
                throw;
            }
        }

        public async Task UpdateLoanAsync(int id, LoanRequestDto loanDto)
        {
            if (loanDto == null)
            {
                _logger.LogError("LoanDTO is null.");
                throw new ArgumentNullException(nameof(loanDto), "LoanDTO cannot be null.");
            }

            if (id <= 0)
            {
                _logger.LogError("Invalid ID provided for UpdateLoanAsync.");
                throw new ArgumentException("ID must be greater than zero.", nameof(id));
            }

            try
            {
                var loan = _mapper.Map<Loan>(loanDto);
                loan.Id = id;
                await _loanRepository.UpdateLoanAsync(id, loan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating loan with ID {id}");
                throw;
            }
        }

        public async Task DeleteLoanAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogError("Invalid ID provided for DeleteLoanAsync.");
                throw new ArgumentException("ID must be greater than zero.", nameof(id));
            }

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
