using LibraryManagementSystem.Core.Models;

namespace LibraryManagementSystem.Core.Interfaces.Repositories
{
    public interface IBooksRepository
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> GetBookByIdAsync(int id);
        Task<Book> AddBookAsync(Book book);
        Task UpdateBookAsync(int id, Book book);
        Task DeleteBookAsync(int id);
    }
}
