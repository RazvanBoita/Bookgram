using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnByDoing.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedWishlistJoinTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WishlistUserBooks",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    BookId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishlistUserBooks", x => new { x.UserId, x.BookId });
                    table.ForeignKey(
                        name: "FK_WishlistUserBooks_AspNetUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WishlistUserBooks_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WishlistUserBooks_BookId",
                table: "WishlistUserBooks",
                column: "BookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WishlistUserBooks");
        }
    }
}
