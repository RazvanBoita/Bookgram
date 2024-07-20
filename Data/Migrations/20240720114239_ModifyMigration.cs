using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnByDoing.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Authors = table.Column<string>(type: "TEXT", nullable: true),
                    AverageRating = table.Column<double>(type: "REAL", nullable: true),
                    Isbn = table.Column<string>(type: "TEXT", nullable: true),
                    Isbn13 = table.Column<double>(type: "REAL", nullable: true),
                    LanguageCode = table.Column<string>(type: "TEXT", nullable: true),
                    NumPages = table.Column<int>(type: "INTEGER", nullable: true),
                    RatingsCount = table.Column<int>(type: "INTEGER", nullable: true),
                    TextReviewsCount = table.Column<int>(type: "INTEGER", nullable: true),
                    PublicationDate = table.Column<string>(type: "TEXT", nullable: true),
                    Publisher = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
