namespace LibraryManagementSystem.Core.DTOs.RequestDtos
{
    public class BookRequestDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public bool IsBorrowed { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
