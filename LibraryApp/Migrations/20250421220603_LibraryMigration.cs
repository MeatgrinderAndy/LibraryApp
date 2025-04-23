using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryApp.Migrations
{
    /// <inheritdoc />
    public partial class LibraryMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateWhenTaken = table.Column<DateOnly>(type: "date", nullable: true),
                    DateWhenNeedToReturn = table.Column<DateOnly>(type: "date", nullable: true),
                    CoverImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    AuthorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Books_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Country", "DateOfBirth", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, "United Kingdom", new DateOnly(1903, 6, 25), "George", "Orwell" },
                    { 2, "United Kingdom", new DateOnly(1965, 7, 31), "J.K.", "Rowling" },
                    { 3, "United States", new DateOnly(1947, 9, 21), "Stephen", "King" },
                    { 4, "United Kingdom", new DateOnly(1890, 9, 15), "Agatha", "Christie" },
                    { 5, "Japan", new DateOnly(1949, 1, 12), "Haruki", "Murakami" },
                    { 6, "United Kingdom", new DateOnly(1775, 12, 16), "Jane", "Austen" },
                    { 7, "United States", new DateOnly(1899, 7, 21), "Ernest", "Hemingway" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, "admin@library.com", "WZRHGrsBESr8wYFZ9sx0tPURuZgG2lmzyvWpwXPKz8U=", "Admin", "admin" },
                    { 2, "john.doe@example.com", "WZRHGrsBESr8wYFZ9sx0tPURuZgG2lmzyvWpwXPKz8U=", "User", "john_doe" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "CoverImage", "DateWhenNeedToReturn", "DateWhenTaken", "Description", "Genre", "ISBN", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, 1, null, new DateOnly(2023, 6, 10), new DateOnly(2023, 5, 10), "Classic dystopian novel about totalitarianism", "Dystopian", "978-0451524935", "1984", 2 },
                    { 2, 1, null, null, null, "Political satire about farm animals", "Satire", "978-0452284234", "Animal Farm", null },
                    { 3, 2, null, new DateOnly(2023, 6, 15), new DateOnly(2023, 5, 15), "First book in the Harry Potter series", "Fantasy", "978-0747532743", "Harry Potter and the Philosopher's Stone", 2 },
                    { 4, 2, null, null, null, "Second book in the Harry Potter series", "Fantasy", "978-0747538486", "Harry Potter and the Chamber of Secrets", null },
                    { 5, 3, null, null, null, "Classic horror novel about a haunted hotel", "Horror", "978-1501142970", "The Shining", null },
                    { 6, 3, null, null, null, "Story about a shape-shifting monster", "Horror", "978-1501175466", "It", null },
                    { 7, 4, null, null, null, "Famous Hercule Poirot mystery", "Mystery", "978-0062073501", "Murder on the Orient Express", null },
                    { 8, 5, null, null, null, "Surreal novel blending reality and fantasy", "Magical Realism", "978-0307476463", "Kafka on the Shore", null },
                    { 9, 6, null, null, null, "Classic romance novel", "Romance", "978-1503290564", "Pride and Prejudice", null },
                    { 10, 7, null, null, null, "Story of an aging fisherman's struggle", "Literary Fiction", "978-0684801469", "The Old Man and the Sea", null },
                    { 11, 3, null, null, null, "Epic novel about a deadly pandemic", "Post-Apocalyptic", "978-1501142971", "The Stand", null },
                    { 12, 5, null, null, null, "Coming-of-age story set in 1960s Tokyo", "Literary Fiction", "978-0099458326", "Norwegian Wood", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_UserId",
                table: "Books",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
