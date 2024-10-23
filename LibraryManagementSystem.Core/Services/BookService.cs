using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagementSystem.Core.DTOs;
using LibraryManagementSystem.Core.Interfaces.Repositories;
using LibraryManagementSystem.Core.Interfaces.Services;
using LibraryManagementSystem.Core.Models;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<BookDTO>> GetAllBooksAsync()
    {
        var books = await _bookRepository.GetAllBooksAsync();
        var bookDtos = new List<BookDTO>();

        foreach (var book in books)
        {
            bookDtos.Add(new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                IsBorrowed = book.IsBorrowed,
                PublishedDate = book.PublishedDate
            });
        }

        return bookDtos;
    }

    public async Task<BookDTO> GetBookByIdAsync(int id)
    {
        var book = await _bookRepository.GetBookByIdAsync(id);
        return new BookDTO
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            IsBorrowed = book.IsBorrowed,
            PublishedDate = book.PublishedDate
        };
    }

    public async Task AddBookAsync(BookDTO bookDto)
    {
        var book = new Book
        {
            Title = bookDto.Title,
            Author = bookDto.Author,
            IsBorrowed = bookDto.IsBorrowed,
            PublishedDate = bookDto.PublishedDate
        };
        await _bookRepository.AddBookAsync(book);
    }

    public async Task UpdateBookAsync(BookDTO bookDto)
    {
        var book = new Book
        {
            Id = bookDto.Id,
            Title = bookDto.Title,
            Author = bookDto.Author,
            IsBorrowed = bookDto.IsBorrowed,
            PublishedDate = bookDto.PublishedDate
        };
        await _bookRepository.UpdateBookAsync(book);
    }

    public async Task DeleteBookAsync(int id)
    {
        await _bookRepository.DeleteBookAsync(id);
    }
}
