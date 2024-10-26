using FluentAssertions;
using LibraryManagementSystem.Core.Interfaces.Repositories;
using LibraryManagementSystem.Core.Models;
using LibraryManagementSystem.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LibraryManagementSystem.UnitTests.ServicesTests
{
    public class NotificationServiceTests
    {
        private readonly Mock<ILoansRepository> _mockLoanRepository;
        private readonly Mock<ILogger<NotificationService>> _mockLogger;

        public NotificationServiceTests()
        {
            _mockLoanRepository = new Mock<ILoansRepository>();
            _mockLogger = new Mock<ILogger<NotificationService>>();
        }

        [Fact]
        public async Task CheckDueDates_LogsNumberOfDueLoansAndSendsNotifications()
        {
            // Arrange
            var expectedUser = new User { FirstName = "John", LastName = "Doe" };
            var expectedLoan = new Loan
            {
                User = expectedUser,
                Book = new Book { Title = "Test Book" },
                DueDate = DateTime.Now.AddDays(1)
            };

            var expectedLoans = new List<Loan> { expectedLoan };

            _mockLoanRepository.Setup(x => x.GetLoansDueTomorrowAsync())
                .ReturnsAsync(expectedLoans.AsEnumerable());

            var notificationService = new NotificationService(_mockLoanRepository.Object, _mockLogger.Object);

            // Act
            await notificationService.CheckDueDates();

            // Assert
            _mockLoanRepository.Verify(x => x.GetLoansDueTomorrowAsync(), Times.Once);

            _mockLogger.Verify(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("1 loan(s) are due tomorrow.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task CheckDueDates_LogsErrorAndRethrowsOnException()
        {
            // Arrange
            _mockLoanRepository.Setup(x => x.GetLoansDueTomorrowAsync())
                .ThrowsAsync(new Exception("Simulated exception"));

            var notificationService = new NotificationService(_mockLoanRepository.Object, _mockLogger.Object);

            // Act
            Func<Task> act = async () => await notificationService.CheckDueDates();

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Simulated exception");

            _mockLoanRepository.Verify(x => x.GetLoansDueTomorrowAsync(), Times.Once);

            _mockLogger.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error sending the notification to the users.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
