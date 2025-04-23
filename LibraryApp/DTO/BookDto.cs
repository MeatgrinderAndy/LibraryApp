namespace LibraryApp.DTO
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Isbn { get; set; } = "";
        public string Description { get; set; }
        public string Genre { get; set;}
        public int AuthorId { get; set; }
        public int UserId { get; set; }

        public byte[]? CoverImage { get; set; }
        public string? AuthorName { get; set; }
        public DateOnly? DateWhenTaken { get; set; }
        public DateOnly? DateWhenNeedToReturn { get; set; }
    }

}
