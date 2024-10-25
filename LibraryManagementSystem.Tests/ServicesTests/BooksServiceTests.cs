using Xunit;
using Moq;
using LibraryManagementSystem.Core.Interfaces.Repositories;
using LibraryManagementSystem.Core.Models;
using LibraryManagementSystem.Infrastructure.Services;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using LibraryManagementSystem.Core.DTOs.RequestDtos;
using LibraryManagementSystem.Core.DTOs.ResponseDtos;

namespace LibraryManagementSystem.UnitTests.ServicesTests
{
    public class BooksServiceTests
    {
        private readonly Mock<IBooksRepository> _mockBookRepository;
        private readonly Mock<ILogger<BooksService>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;
        private readonly BooksService _bookService;

        public BooksServiceTests()
        {
            _mockBookRepository = new Mock<IBooksRepository>();
            _mockLogger = new Mock<ILogger<BooksService>>();
            _mockMapper = new Mock<IMapper>();

            _bookService = new BooksService(
                _mockBookRepository.Object,
                _mockLogger.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task GetAllBooksAsync_ShouldReturnBooks_WhenBooksExist()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Book 1", Author = "Author 1" },
                new Book { Id = 2, Title = "Book 2", Author = "Author 2" }
            };

            var bookResponseDtos = new List<BookResponseDto>
            {
                new BookResponseDto { Id = 1, Title = "Book 1", Author = "Author 1" },
                new BookResponseDto { Id = 2, Title = "Book 2", Author = "Author 2" }
            };

            _mockBookRepository
                .Setup(repo => repo.GetAllBooksAsync())
                .ReturnsAsync(books);

            _mockMapper
                .Setup(m => m.Map<IEnumerable<BookRequestDto>>(books))
                .Returns(bookResponseDtos);


            // Act
            var result = await _bookService.GetAllBooksAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().ContainSingle(b => b.Title == "Book 1");
            result.Should().ContainSingle(b => b.Title == "Book 2");

            _mockBookRepository.Verify(repo => repo.GetAllBooksAsync(), Times.Once);
            _mockMapper.Verify(m => m.Map<IEnumerable<BookRequestDto>>(books), Times.Once);
        }

        [Fact]
        public async Task GetAllBooksAsync_ShouldLogError_WhenRepositoryThrowsException()
        {
            // Arrange
            var exception = new Exception("Database error");
            _mockBookRepository
                .Setup(repo => repo.GetAllBooksAsync())
                .ThrowsAsync(exception);  
            // Act
            Func<Task> action = async () => await _bookService.GetAllBooksAsync();

            // Assert
            await action.Should().ThrowAsync<Exception>().WithMessage("Database error");
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error retrieving all books.")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task GetBookByIdAsync_ShouldReturnBook_WhenBookExists()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Book 1", Author = "Author 1" };

            _mockBookRepository
                .Setup(repo => repo.GetBookByIdAsync(1))
                .ReturnsAsync(book);

            _mockMapper
                .Setup(m => m.Map<BookRequestDto>(book))
                .Returns(new BookResponseDto { Title = "Book 1", Author = "Author 1" });

            // Act
            var result = await _bookService.GetBookByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be("Book 1");

            _mockBookRepository.Verify(repo => repo.GetBookByIdAsync(1), Times.Once);
            _mockMapper.Verify(m => m.Map<BookRequestDto>(book), Times.Once);
        }

        [Fact]
        public async Task GetBookByIdAsync_ShouldReturnNull_WhenBookDoesNotExist()
        {
            // Arrange
            _mockBookRepository
                .Setup(repo => repo.GetBookByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Book)null);

            // Act
            var result = await _bookService.GetBookByIdAsync(999);

            // Assert
            result.Should().BeNull();
            _mockBookRepository.Verify(repo => repo.GetBookByIdAsync(999), Times.Once);
        }

        [Fact]
        public async Task GetBookByIdAsync_ShouldThrowArgumentException_WhenBookIdIsInvalid()
        {
            // Arrange
            int id = 0;

            // Act
            Func<Task> action = async () => await _bookService.GetBookByIdAsync(id);

            // Assert
            var exception = await action.Should().ThrowAsync<ArgumentException>();
                exception.WithMessage("ID must be greater than zero.*")
                      .And.ParamName.Should().Be(nameof(id)); 

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Invalid ID provided for GetBookByIdAsync.")),
                    It.IsAny<ArgumentException>(), 
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task GetBookByIdAsync_ShouldLogError_WhenRepositoryThrowsException()
        {
            // Arrange
            var exception = new Exception("Database error");
            _mockBookRepository
                .Setup(repo => repo.GetBookByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(exception);  // Simulate an exception

            // Act
            Func<Task> action = async () => await _bookService.GetBookByIdAsync(1);

            // Assert
            await action.Should().ThrowAsync<Exception>().WithMessage("Database error");
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error retrieving book with ID 1")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ),
                Times.Once
            );
        }

        
        [Fact]
        public async Task AddBookAsync_ShouldInvokeRepository_WhenBookDtoIsValid()
        {
            // Arrange
            var bookDto = new BookRequestDto { Title = "New Book", Author = "New Author" };
            var book = new Book { Title = "New Book", Author = "New Author" };

            _mockMapper
                .Setup(m => m.Map<Book>(bookDto))
                .Returns(book);

            _mockBookRepository
                .Setup(repo => repo.AddBookAsync(book))
                .ReturnsAsync(book);

            // Act
            await _bookService.AddBookAsync(bookDto);

            // Assert
            _mockMapper.Verify(m => m.Map<Book>(bookDto), Times.Once);
            _mockBookRepository.Verify(repo => repo.AddBookAsync(book), Times.Once);
        }

        [Fact]
        public async Task AddBookAsync_ShouldThrowArgumentNullException_WhenBookDtoIsNull()
        {
            // Arrange
            BookRequestDto bookDto = null;

            // Act
            Func<Task> action = async () => await _bookService.AddBookAsync(bookDto);

            // Assert
            var exception = await action.Should().ThrowAsync<ArgumentNullException>();
            exception.WithMessage("BookDTO cannot be null.*")
                      .And.ParamName.Should().Be(nameof(bookDto)); 

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("BookDTO is null.")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task AddBookAsync_ShouldLogError_WhenRepositoryThrowsException()
        {
            // Arrange
            var bookDto = new BookRequestDto { Title = "Invalid Book", Author = "Error Author" };
            var exception = new Exception("Repository error");

            _mockMapper
                .Setup(m => m.Map<Book>(bookDto))
                .Returns(new Book { Title = "Invalid Book", Author = "Error Author" });

            _mockBookRepository
                .Setup(repo => repo.AddBookAsync(It.IsAny<Book>()))
                .ThrowsAsync(exception);

            // Act
            Func<Task> action = async () => await _bookService.AddBookAsync(bookDto);

            // Assert
            await action.Should().ThrowAsync<Exception>().WithMessage("Repository error");
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Error adding book {bookDto.Title} by {bookDto.Author}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task UpdateBookAsync_ShouldInvokeRepository_WhenBookDtoIsValid()
        {
            // Arrange
            var id = 1;
            var bookDto = new BookRequestDto { Title = "Updated Book", Author = "Updated Author" };
            var book = new Book { Id = 1, Title = "Updated Book", Author = "Updated Author" };

            _mockMapper
                .Setup(m => m.Map<Book>(bookDto))
                .Returns(book);

            _mockBookRepository
                .Setup(repo => repo.UpdateBookAsync(id, book))
                .Returns(Task.CompletedTask);

            // Act
            await _bookService.UpdateBookAsync(id, bookDto);

            // Assert
            _mockMapper.Verify(m => m.Map<Book>(bookDto), Times.Once);
            _mockBookRepository.Verify(repo => repo.UpdateBookAsync(id, book), Times.Once);
        }

        [Fact]
        public async Task UpdateBookAsync_ShouldLogError_WhenRepositoryThrowsException()
        {
            // Arrange
            var id = 1;
            var bookDto = new BookRequestDto { Title = "Updated Book", Author = "Updated Author" };
            var exception = new Exception("Repository update error");

            _mockMapper
                .Setup(m => m.Map<Book>(bookDto))
                .Returns(new Book { Title = "Updated Book", Author = "Updated Author" });

            _mockBookRepository
                .Setup(repo => repo.UpdateBookAsync(It.IsAny<int>(), It.IsAny<Book>()))
                .ThrowsAsync(exception);

            // Act
            Func<Task> action = async () => await _bookService.UpdateBookAsync(id, bookDto);

            // Assert
            await action.Should().ThrowAsync<Exception>().WithMessage("Repository update error");

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Error updating book with ID {id}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task UpdateBookAsync_ShouldThrowArgumentNullException_WhenBookDtoIsNull()
        {
            // Arrange
            var id = 1;
            BookRequestDto bookDto = null;

            // Act
            Func<Task> action = async () => await _bookService.UpdateBookAsync(id, bookDto);

            // Assert
            var exception = await action.Should().ThrowAsync<ArgumentNullException>();
            exception.WithMessage("BookDTO cannot be null.*")
                      .And.ParamName.Should().Be(nameof(bookDto)); // Use nameof for parameter name

            // Verify logging
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("BookDTO is null.")),
                    It.IsAny<ArgumentNullException>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ),
                Times.Once
            );
        }

        //[Fact]
        //public async Task UpdateBookAsync_ShouldThrowArgumentException_WhenBookIdIsInvalid()
        //{
        //    // Arrange
        //    var id = -12;
        //    var invalidBookDto = new BookRequestDto {}; 

        //    // Act
        //    Func<Task> action = async () => await _bookService.UpdateBookAsync(invalidBookDto);

        //    // Assert
        //    var exception = await action.Should().ThrowAsync<ArgumentException>();

        //    exception.WithMessage("ID must be greater than zero.*")
        //              .And.ParamName.Should().Be(nameof(invalidBookDto.Id)); 

        //    _mockLogger.Verify(
        //        x => x.Log(
        //            LogLevel.Error,
        //            It.IsAny<EventId>(),
        //            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Invalid ID provided for UpdateBookAsync.")),
        //            It.IsAny<ArgumentException>(), 
        //            It.IsAny<Func<It.IsAnyType, Exception, string>>()
        //        ),
        //        Times.Once
        //    );
        //}

        [Fact]
        public async Task DeleteBookAsync_ShouldInvokeRepository_WhenBookIdIsValid()
        {
            // Arrange
            var bookId = 1;

            _mockBookRepository
                .Setup(repo => repo.DeleteBookAsync(bookId))
                .Returns(Task.CompletedTask);

            // Act
            await _bookService.DeleteBookAsync(bookId);

            // Assert
            _mockBookRepository.Verify(repo => repo.DeleteBookAsync(bookId), Times.Once);
        }

        [Fact]
        public async Task DeleteBookAsync_ShouldLogError_WhenRepositoryThrowsException()
        {
            // Arrange
            var bookId = 1;
            var exception = new Exception("Repository delete error");

            _mockBookRepository
                .Setup(repo => repo.DeleteBookAsync(bookId))
                .ThrowsAsync(exception);

            // Act
            Func<Task> action = async () => await _bookService.DeleteBookAsync(bookId);

            // Assert
            await action.Should().ThrowAsync<Exception>().WithMessage("Repository delete error");

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Error deleting book with ID {bookId}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task DeleteBookAsync_ShouldThrowArgumentException_WhenBookIdIsInvalid()
        {
            // Arrange
            int id = 0;

            // Act
            Func<Task> action = async () => await _bookService.DeleteBookAsync(id);

            // Assert
            var exception = await action.Should().ThrowAsync<ArgumentException>();

                exception.WithMessage("ID must be greater than zero.*")
                      .And.ParamName.Should().Be(nameof(id));

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Invalid ID provided for DeleteBookAsync.")),
                    It.IsAny<ArgumentException>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ),
                Times.Once
            );
        }

    }
}
