using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReviewToBytes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachedImagePath",
                table: "Reviews");

            migrationBuilder.AddColumn<byte[]>(
                name: "AttachedImageData",
                table: "Reviews",
                type: "BLOB",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachedImageData",
                table: "Reviews");

            migrationBuilder.AddColumn<string>(
                name: "AttachedImagePath",
                table: "Reviews",
                type: "TEXT",
                nullable: true);
        }
    }
}
