using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using LibraryManagementSystem.Core.DTOs.RequestDtos;
using LibraryManagementSystem.Core.DTOs.ResponseDtos;
using LibraryManagementSystem.Core.Interfaces.Repositories;
using LibraryManagementSystem.Core.Models;
using LibraryManagementSystem.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace LibraryManagementSystem.UnitTests.ServicesTests
{
    public class LoansServiceTests
    {
        private readonly Mock<ILoansRepository> _mockLoanRepository;
        private readonly Mock<ILogger<LoansService>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;
        private readonly LoansService _loanService;

        public LoansServiceTests()
        {
            _mockLoanRepository = new Mock<ILoansRepository>();
            _mockLogger = new Mock<ILogger<LoansService>>();
            _mockMapper = new Mock<IMapper>();

            _loanService = new LoansService(_mockLoanRepository.Object, _mockLogger.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllLoansAsync_ShouldReturnLoanDTOs()
        {
            // Arrange
            var loans = new List<Loan>
            {
                new Loan { Id = 1, BookId = 1, UserId = 1 },
                new Loan { Id = 2, BookId = 2, UserId = 2 }
            };
            var loanDtos = new List<LoanResponseDto>
            {
                new LoanResponseDto {  BookId = 1, UserId = 1 },
                new LoanResponseDto { BookId = 2, UserId = 2 }
            };

            _mockLoanRepository.Setup(repo => repo.GetAllLoansAsync()).ReturnsAsync(loans);
            _mockMapper.Setup(m => m.Map<IEnumerable<LoanResponseDto>>(loans)).Returns(loanDtos);

            // Act
            var result = await _loanService.GetAllLoansAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().ContainSingle(l => l.BookId == 1 && l.UserId == 1);
            result.Should().ContainSingle(l => l.BookId == 2 && l.UserId == 2);
        }

        [Fact]
        public async Task GetLoanByIdAsync_ShouldReturnLoanDTO_WhenIdIsValid()
        {
            // Arrange
            var loan = new Loan { Id = 1, BookId = 1, UserId = 1 };
            var loanDto = new LoanResponseDto { BookId = 1, UserId = 1 };

            _mockLoanRepository.Setup(repo => repo.GetLoanByIdAsync(1)).ReturnsAsync(loan);
            _mockMapper.Setup(m => m.Map<LoanResponseDto>(loan)).Returns(loanDto);

            // Act
            var result = await _loanService.GetLoanByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.BookId.Should().Be(1);
            result.UserId.Should().Be(1);
        }

        [Fact]
        public async Task AddLoanAsync_ShouldAddLoan_WhenLoanDtoIsValid()
        {
            // Arrange
            var loanDto = new LoanRequestDto { BookId = 1, UserId = 1 };
            var loan = new Loan { Id = 1, BookId = 1, UserId = 1 };

            _mockMapper.Setup(m => m.Map<Loan>(loanDto)).Returns(loan);
            _mockLoanRepository.Setup(repo => repo.AddLoanAsync(loan)).ReturnsAsync(loan);

            // Act
            await _loanService.AddLoanAsync(loanDto);

            // Assert
            _mockLoanRepository.Verify(repo => repo.AddLoanAsync(It.IsAny<Loan>()), Times.Once);
        }

        [Fact]
        public async Task UpdateLoanAsync_ShouldUpdateLoan_WhenLoanDtoIsValid()
        {
            // Arrange

            var id = 1;
            var loanDto = new LoanRequestDto { BookId = 1, UserId = 1 };
            var loan = new Loan { Id = 1, BookId = 1, UserId = 1 };

            _mockMapper.Setup(m => m.Map<Loan>(loanDto)).Returns(loan);
            _mockLoanRepository.Setup(repo => repo.UpdateLoanAsync(id, loan)).Returns(Task.CompletedTask);

            // Act
            await _loanService.UpdateLoanAsync(id, loanDto);

            // Assert
            _mockLoanRepository.Verify(repo => repo.UpdateLoanAsync(It.IsAny<int>(), It.IsAny<Loan>()), Times.Once);
        }

        [Fact]
        public async Task DeleteLoanAsync_ShouldDeleteLoan_WhenIdIsValid()
        {
            // Arrange
            var loanId = 1;
            _mockLoanRepository.Setup(repo => repo.DeleteLoanAsync(loanId)).Returns(Task.CompletedTask);

            // Act
            await _loanService.DeleteLoanAsync(loanId);

            // Assert
            _mockLoanRepository.Verify(repo => repo.DeleteLoanAsync(loanId), Times.Once);
        }
    }
}
