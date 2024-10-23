using LibraryManagementSystem.Core.DTOs;
using LibraryManagementSystem.Core.Interfaces.Repositories;
using LibraryManagementSystem.Core.Interfaces.Services;
using LibraryManagementSystem.Core.Models;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace LibraryManagementSystem.Infrastructure.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<BookService> _logger;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, ILogger<BookService> logger, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _logger = logger;
            _mapper = mapper; 
        }

        public async Task<IEnumerable<BookDTO>> GetAllBooksAsync()
        {
            try
            {
                var books = await _bookRepository.GetAllBooksAsync();
                return _mapper.Map<IEnumerable<BookDTO>>(books); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all books.");
                throw;
            }
        }

        public async Task<BookDTO> GetBookByIdAsync(int id)
        {
            try
            {
                var book = await _bookRepository.GetBookByIdAsync(id);
                return _mapper.Map<BookDTO>(book); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving book with ID {id}");
                throw;
            }
        }

        public async Task AddBookAsync(BookDTO bookDto)
        {
            try
            {
                var book = _mapper.Map<Book>(bookDto); 
                await _bookRepository.AddBookAsync(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding book {bookDto.Title} by {bookDto.Author}");
                throw;
            }
        }

        public async Task UpdateBookAsync(BookDTO bookDto)
        {
            try
            {
                var book = _mapper.Map<Book>(bookDto); 
                await _bookRepository.UpdateBookAsync(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating book with ID {bookDto.Id}");
                throw;
            }
        }

        public async Task DeleteBookAsync(int id)
        {
            try
            {
                await _bookRepository.DeleteBookAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting book with ID {id}");
                throw;
            }
        }
    }
}
