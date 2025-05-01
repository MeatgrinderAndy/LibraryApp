namespace LibraryApp.Domain.Entities;

public class Book
{
    public int Id { get; set; }
    public string ISBN { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public string Description { get; set; }
    public DateOnly? DateWhenTaken { get; set; }
    public DateOnly? DateWhenNeedToReturn { get; set; }
    public byte[]? CoverImage { get; set; }
    public int? UserId { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; }
    public User User { get; set; }

}

