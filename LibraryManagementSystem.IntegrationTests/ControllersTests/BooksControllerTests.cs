using FluentAssertions;
using LibraryManagementSystem.Core.DTOs.RequestDtos;
using LibraryManagementSystem.Core.DTOs.ResponseDtos;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace LibraryManagementSystem.IntegrationTests.ControllersTests
{
    public class BooksControllerTests : BaseControllerTests
    {
        public BooksControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetBooks_ReturnsOkResult()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/books");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var books = await response.Content.ReadAsAsync<IEnumerable<BookResponseDto>>();
            books.Should().NotBeEmpty();
        }

        [Fact(Skip = "Only local testing")]
        public async Task CreateBook_ReturnsCreatedResult()
        {
            // Arrange
            var newBook = new BookRequestDto
            {
                Title = "New Book",
                Author = "Author Name",
                PublishedDate = DateTime.Now,
                IsBorrowed = false,

            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/books", newBook);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var createdBook = await response.Content.ReadAsAsync<BookResponseDto>();
            createdBook.Should().NotBeNull();
            createdBook.Title.Should().Be(newBook.Title);
        }

        [Fact]
        public async Task GetBook_ReturnsOkResult_WhenBookExists()
        {
            // Arrange
            var existingBookId = 1; 

            // Act
            var response = await _client.GetAsync($"/api/v1/books/{existingBookId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var book = await response.Content.ReadAsAsync<BookResponseDto>();
            book.Should().NotBeNull();
            book.Id.Should().Be(existingBookId);
        }

        [Fact]
        public async Task GetBook_ReturnsNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            var nonExistentBookId = 999; 

            // Act
            var response = await _client.GetAsync($"/api/v1/books/{nonExistentBookId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact(Skip = "Only local testing")]
        public async Task UpdateBook_ReturnsNoContent()
        {
            // Arrange
            var bookIdToUpdate = 1; 
            var updatedBook = new BookRequestDto
            {
                Title = "Updated Book",
                Author = "Updated Author"
                // Add other required properties
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/v1/books/{bookIdToUpdate}", updatedBook);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact(Skip = "Only local testing")]
        public async Task DeleteBook_ReturnsNoContent_WhenBookExists()
        {
            // Arrange
            var bookIdToDelete = 1; 

            // Act
            var response = await _client.DeleteAsync($"/api/v1/books/{bookIdToDelete}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteBook_ReturnsNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            var nonExistentBookId = 999; 

            // Act
            var response = await _client.DeleteAsync($"/api/v1/books/{nonExistentBookId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
