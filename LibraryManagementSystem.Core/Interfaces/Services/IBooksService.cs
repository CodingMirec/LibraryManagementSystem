using LibraryManagementSystem.Core.DTOs.RequestDtos;
using LibraryManagementSystem.Core.DTOs.ResponseDtos;

namespace LibraryManagementSystem.Core.Interfaces.Services
{
    public interface IBooksService
    {
        Task<IEnumerable<BookResponseDto>> GetAllBooksAsync();
        Task<BookResponseDto> GetBookByIdAsync(int id);
        Task<BookResponseDto> AddBookAsync(BookRequestDto bookDto);
        Task UpdateBookAsync(int id, BookRequestDto bookDto);
        Task DeleteBookAsync(int id);
    }
}
