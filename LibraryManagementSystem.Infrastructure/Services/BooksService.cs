using LibraryManagementSystem.Core.Interfaces.Repositories;
using LibraryManagementSystem.Core.Interfaces.Services;
using LibraryManagementSystem.Core.Models;
using Microsoft.Extensions.Logging;
using AutoMapper;
using LibraryManagementSystem.Core.DTOs.RequestDtos;
using LibraryManagementSystem.Core.DTOs.ResponseDtos;

namespace LibraryManagementSystem.Infrastructure.Services
{
    public class BooksService : IBooksService
    {
        private readonly IBooksRepository _bookRepository;
        private readonly ILogger<BooksService> _logger;
        private readonly IMapper _mapper;

        public BooksService(IBooksRepository bookRepository, ILogger<BooksService> logger, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _logger = logger;
            _mapper = mapper; 
        }

        public async Task<IEnumerable<BookResponseDto>> GetAllBooksAsync()
        {
            try
            {
                var books = await _bookRepository.GetAllBooksAsync();
                return _mapper.Map<IEnumerable<BookResponseDto>>(books); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all books.");
                throw;
            }
        }

        public async Task<BookResponseDto> GetBookByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogError("Invalid ID provided for GetBookByIdAsync.");
                throw new ArgumentException("ID must be greater than zero.", nameof(id));
            }

            try
            {
                var book = await _bookRepository.GetBookByIdAsync(id);
                return _mapper.Map<BookResponseDto>(book); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving book with ID {id}");
                throw;
            }
        }

        public async Task<BookResponseDto> AddBookAsync(BookRequestDto bookDto)
        {
            if (bookDto == null)
            {
                _logger.LogError("BookDTO is null.");
                throw new ArgumentNullException(nameof(bookDto), "BookDTO cannot be null.");
            }

            try
            {
                var book = _mapper.Map<Book>(bookDto); 
                await _bookRepository.AddBookAsync(book);

                var bookResponseDto = _mapper.Map<BookResponseDto>(book);
                return bookResponseDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding book {bookDto.Title} by {bookDto.Author}");
                throw;
            }
        }

        public async Task<BookResponseDto> UpdateBookAsync(int id, BookRequestDto bookDto)
        {
            if (bookDto == null)
            {
                _logger.LogError("BookDTO is null.");
                throw new ArgumentNullException(nameof(bookDto), "BookDTO cannot be null.");
            }

            if (id <= 0)
            {
                _logger.LogError("Invalid ID provided for UpdateBookAsync.");
                throw new ArgumentException("ID must be greater than zero.", nameof(id));
            }

            try
            {
                var book = _mapper.Map<Book>(bookDto);
                await _bookRepository.UpdateBookAsync(id, book);

                var bookResponseDto = _mapper.Map<BookResponseDto>(book);
                return bookResponseDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating book with ID {id}");
                throw;
            }
        }

        public async Task<BookResponseDto> DeleteBookAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogError("Invalid ID provided for DeleteBookAsync.");
                throw new ArgumentException("ID must be greater than zero.", nameof(id));
            }

            try
            {
                var book = await _bookRepository.DeleteBookAsync(id);
                var bookResponseDto = _mapper.Map<BookResponseDto>(book);

                return bookResponseDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting book with ID {id}");
                throw;
            }
        }
    }
}
