using FluentAssertions;
using LibraryManagementSystem.Core.DTOs.RequestDtos;
using LibraryManagementSystem.Core.DTOs.ResponseDtos;
using System.Net;
using System.Net.Http.Json;
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
            var response = await _client.GetAsync("/api/v1/books");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var books = await response.Content.ReadAsAsync<IEnumerable<BookResponseDto>>();
            books.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetBookById_ReturnsOkResult_WhenBookExists()
        {
            var existingBookId = 1;

            var response = await _client.GetAsync($"/api/v1/books/{existingBookId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var book = await response.Content.ReadAsAsync<BookResponseDto>();
            book.Should().NotBeNull();
            book.Id.Should().Be(existingBookId);
        }

        [Fact]
        public async Task GetBook_ReturnsNotFound_WhenBookDoesNotExist()
        {
            var nonExistentBookId = 999;

            var response = await _client.GetAsync($"/api/v1/books/{nonExistentBookId}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact(Skip = "Only local testing")]
        public async Task CreateBook_ReturnsCreatedResult()
        {
            var newBook = new BookRequestDto
            {
                Title = "New Book",
                Author = "Author Name",
                PublishedDate = DateTime.Now,
                IsBorrowed = false,
            };

            var response = await _client.PostAsJsonAsync("/api/v1/books", newBook);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();

            var createdBook = await response.Content.ReadAsAsync<BookResponseDto>();
            createdBook.Should().NotBeNull();
            createdBook.Title.Should().Be(newBook.Title);
        }

        [Fact(Skip = "Only local testing")]
        public async Task UpdateBook_ReturnsOkResult_WhenBookExists()
        {
            var bookIdToUpdate = 1;
            var updatedBook = new BookRequestDto
            {
                Title = "Updated Book",
                Author = "Updated Author",
                PublishedDate = DateTime.Now,
                IsBorrowed = false
            };

            var response = await _client.PutAsJsonAsync($"/api/v1/books/{bookIdToUpdate}", updatedBook);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var returnedBook = await response.Content.ReadAsAsync<BookResponseDto>();
            returnedBook.Should().NotBeNull();
            returnedBook.Title.Should().Be(updatedBook.Title);
        }

        [Fact(Skip = "Only local testing")]
        public async Task DeleteBook_ReturnsOkResult_WhenBookExists()
        {
            var bookIdToDelete = 1;

            var response = await _client.DeleteAsync($"/api/v1/books/{bookIdToDelete}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var deletedBook = await response.Content.ReadAsAsync<BookResponseDto>();
            deletedBook.Should().NotBeNull();
            deletedBook.Id.Should().Be(bookIdToDelete);
        }

        [Fact]
        public async Task DeleteBook_ReturnsNotFound_WhenBookDoesNotExist()
        {
            var nonExistentBookId = 9999;

            var response = await _client.DeleteAsync($"/api/v1/books/{nonExistentBookId}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
