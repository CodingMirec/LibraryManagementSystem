using LibraryManagementSystem.Core.DTOs;

namespace LibraryManagementSystem.Core.Interfaces.Services
{
    public interface IBookService
    {
        Task<IEnumerable<BookDTO>> GetAllBooksAsync();
        Task<BookDTO> GetBookByIdAsync(int id);
        Task AddBookAsync(BookDTO bookDto);
        Task UpdateBookAsync(BookDTO bookDto);
        Task DeleteBookAsync(int id);
    }
}
