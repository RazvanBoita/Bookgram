using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnByDoing.Data.Migrations
{
    /// <inheritdoc />
    public partial class MapUserToBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "num_pages",
                table: "Books",
                newName: "NumPages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumPages",
                table: "Books",
                newName: "num_pages");
        }
    }
}
