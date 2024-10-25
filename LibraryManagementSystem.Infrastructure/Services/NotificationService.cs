using LibraryManagementSystem.Core.Models;
using LibraryManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure.Services
{
    public class NotificationService
    {
        private readonly LibraryDbContext _context;

        public NotificationService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task CheckDueDates()
        {
            var dueLoans = await _context.Loans
                .Where(loan => loan.DueDate.Date == DateTime.Now.AddDays(1).Date)
                .Include(loan => loan.User) 
                .Include(loan => loan.Book)
                .ToListAsync();

            foreach (var loan in dueLoans)
            {
                SimulateSendNotification(loan.User, loan);
            }
        }

        private void SimulateSendNotification(User user, Loan loan)
        {
            Console.WriteLine($"Reminder: {user.FirstName} {user.LastName}, your loan for '{loan.Book.Title}' is due tomorrow ({loan.DueDate.ToShortDateString()}).");
        }
    }
}
