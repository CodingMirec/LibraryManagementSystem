﻿using LibraryManagementSystem.Core.Interfaces.Repositories;
using LibraryManagementSystem.Core.Models;
using LibraryManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;
        private readonly ILogger<BookRepository> _logger;

        public BookRepository(LibraryDbContext context, ILogger<BookRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            try
            {
                return await _context.Books.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all books");
                throw; 
            }
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            try
            {
                return await _context.Books.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving book with ID {id}");
                throw; 
            }
        }

        public async Task AddBookAsync(Book book)
        {
            try
            {
                await _context.Books.AddAsync(book);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding a new book");
                throw; 
            }
        }

        public async Task UpdateBookAsync(Book book)
        {
            try
            {
                _context.Books.Update(book);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating the book");
                throw; 
            }
        }

        public async Task DeleteBookAsync(int id)
        {
            try
            {
                var book = await GetBookByIdAsync(id);
                if (book != null)
                {
                    _context.Books.Remove(book);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _logger.LogWarning($"Book with ID {id} not found for deletion");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting book with ID {id}");
                throw; 
            }
        }
    }
}