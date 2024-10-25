using LibraryManagementSystem.Core.Models;

namespace LibraryManagementSystem.Core.Interfaces.Repositories
{
    public interface IBooksRepository
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> GetBookByIdAsync(int id);
        Task<Book> AddBookAsync(Book book);
        Task<Book> UpdateBookAsync(int id, Book book);
        Task<Book> DeleteBookAsync(int id);
    }
}
