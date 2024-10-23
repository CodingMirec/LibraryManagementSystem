namespace LibraryManagementSystem.Core.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public bool IsBorrowed { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
