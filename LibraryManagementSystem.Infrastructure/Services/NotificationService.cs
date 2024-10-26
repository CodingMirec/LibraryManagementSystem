using LibraryManagementSystem.Core.Interfaces.Repositories;
using LibraryManagementSystem.Core.Interfaces.Services;
using LibraryManagementSystem.Core.Models;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILoansRepository _loanRepository;
        private readonly ILogger<NotificationService> _logger;


        public NotificationService(ILoansRepository loanRepository, ILogger<NotificationService>  logger)
        {
            _loanRepository = loanRepository;
            _logger = logger;
        }

        public async Task CheckDueDates()
        {
            try
            {
                var dueLoans = await _loanRepository.GetLoansDueTomorrowAsync();
                _logger.LogInformation("{Count} loan(s) are due tomorrow.", dueLoans.Count());
                foreach (var loan in dueLoans)
                {
                    SimulateSendNotification(loan.User, loan);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error sending the notification to the users.");
                throw;
            }
            
        }

        private void SimulateSendNotification(User user, Loan loan)
        {
            Console.WriteLine($"Reminder: {user.FirstName} {user.LastName}, your loan for '{loan.Book.Title}' is due tomorrow ({loan.DueDate.ToShortDateString()}).");
        }
    }
}
