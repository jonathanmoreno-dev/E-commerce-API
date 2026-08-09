using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserIdIndexAsUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_checkouts_user_id",
                table: "checkouts");

            migrationBuilder.CreateIndex(
                name: "IX_checkouts_user_id",
                table: "checkouts",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_checkouts_user_id",
                table: "checkouts");

            migrationBuilder.CreateIndex(
                name: "IX_checkouts_user_id",
                table: "checkouts",
                column: "user_id",
                unique: true);
        }
    }
}
