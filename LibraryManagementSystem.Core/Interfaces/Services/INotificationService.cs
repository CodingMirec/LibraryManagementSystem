using LibraryManagementSystem.Core.DTOs.RequestDtos;
namespace LibraryManagementSystem.Core.Interfaces.Services
{
    public interface INotificationService
    {
        Task CheckDueDates();
    }
}
