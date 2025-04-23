using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Data;

public class AppDbContext : DbContext
    {
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin", Email = "admin@library.com", PasswordHash = "WZRHGrsBESr8wYFZ9sx0tPURuZgG2lmzyvWpwXPKz8U=", Role = "Admin" },
            new User { Id = 2, Username = "john_doe", Email = "john.doe@example.com", PasswordHash = "WZRHGrsBESr8wYFZ9sx0tPURuZgG2lmzyvWpwXPKz8U=", Role = "User" }
        );

        modelBuilder.Entity<Author>().HasData(
            new Author { Id = 1, FirstName = "George", LastName = "Orwell", DateOfBirth = new DateOnly(1903, 6, 25), Country = "United Kingdom" },
            new Author { Id = 2, FirstName = "J.K.", LastName = "Rowling", DateOfBirth = new DateOnly(1965, 7, 31), Country = "United Kingdom" },
            new Author { Id = 3, FirstName = "Stephen", LastName = "King", DateOfBirth = new DateOnly(1947, 9, 21), Country = "United States" },
            new Author { Id = 4, FirstName = "Agatha", LastName = "Christie", DateOfBirth = new DateOnly(1890, 9, 15), Country = "United Kingdom" },
            new Author { Id = 5, FirstName = "Haruki", LastName = "Murakami", DateOfBirth = new DateOnly(1949, 1, 12), Country = "Japan" },
            new Author { Id = 6, FirstName = "Jane", LastName = "Austen", DateOfBirth = new DateOnly(1775, 12, 16), Country = "United Kingdom" },
            new Author { Id = 7, FirstName = "Ernest", LastName = "Hemingway", DateOfBirth = new DateOnly(1899, 7, 21), Country = "United States" }
        );

        modelBuilder.Entity<Book>().HasData(
            new Book { Id = 1, AuthorId = 1, ISBN = "978-0451524935", Title = "1984", Genre = "Dystopian", Description = "Classic dystopian novel about totalitarianism", DateWhenTaken = new DateOnly(2023, 5, 10), DateWhenNeedToReturn = new DateOnly(2023, 6, 10), UserId = 2 },
            new Book { Id = 2, AuthorId = 1, ISBN = "978-0452284234", Title = "Animal Farm", Genre = "Satire", Description = "Political satire about farm animals" },
            new Book { Id = 3, AuthorId = 2, ISBN = "978-0747532743", Title = "Harry Potter and the Philosopher's Stone", Genre = "Fantasy", Description = "First book in the Harry Potter series", DateWhenTaken = new DateOnly(2023, 5, 15), DateWhenNeedToReturn = new DateOnly(2023, 6, 15), UserId = 2 },
            new Book { Id = 4, AuthorId = 2, ISBN = "978-0747538486", Title = "Harry Potter and the Chamber of Secrets", Genre = "Fantasy", Description = "Second book in the Harry Potter series" },
            new Book { Id = 5, AuthorId = 3, ISBN = "978-1501142970", Title = "The Shining", Genre = "Horror", Description = "Classic horror novel about a haunted hotel" },
            new Book { Id = 6, AuthorId = 3, ISBN = "978-1501175466", Title = "It", Genre = "Horror", Description = "Story about a shape-shifting monster"},
            new Book { Id = 7, AuthorId = 4, ISBN = "978-0062073501", Title = "Murder on the Orient Express", Genre = "Mystery", Description = "Famous Hercule Poirot mystery" },
            new Book { Id = 8, AuthorId = 5, ISBN = "978-0307476463", Title = "Kafka on the Shore", Genre = "Magical Realism", Description = "Surreal novel blending reality and fantasy"},
            new Book { Id = 9, AuthorId = 6, ISBN = "978-1503290564", Title = "Pride and Prejudice", Genre = "Romance", Description = "Classic romance novel" },
            new Book { Id = 10, AuthorId = 7, ISBN = "978-0684801469", Title = "The Old Man and the Sea", Genre = "Literary Fiction", Description = "Story of an aging fisherman's struggle" },
            new Book { Id = 11, AuthorId = 3, ISBN = "978-1501142971", Title = "The Stand", Genre = "Post-Apocalyptic", Description = "Epic novel about a deadly pandemic" },
            new Book { Id = 12, AuthorId = 5, ISBN = "978-0099458326", Title = "Norwegian Wood", Genre = "Literary Fiction", Description = "Coming-of-age story set in 1960s Tokyo" }
        );

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

}

