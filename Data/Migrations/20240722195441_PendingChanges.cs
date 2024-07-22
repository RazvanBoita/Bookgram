using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnByDoing.Data.Migrations
{
    /// <inheritdoc />
    public partial class PendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TextReviewsCount",
                table: "Books",
                newName: "text_reviews_count");

            migrationBuilder.RenameColumn(
                name: "RatingsCount",
                table: "Books",
                newName: "ratings_count");

            migrationBuilder.RenameColumn(
                name: "PublicationDate",
                table: "Books",
                newName: "publication_date");

            migrationBuilder.RenameColumn(
                name: "NumPages",
                table: "Books",
                newName: "num_pages");

            migrationBuilder.RenameColumn(
                name: "LanguageCode",
                table: "Books",
                newName: "language_code");

            migrationBuilder.RenameColumn(
                name: "AverageRating",
                table: "Books",
                newName: "average_rating");
        }
    }
}
