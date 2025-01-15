using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookTheWeek.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddInternalImageUrlToRecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Recipes",
                newName: "ExternalImageUrl");

            migrationBuilder.AddColumn<string>(
                name: "InternalImageUrl",
                table: "Recipes",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true,
                comment: "Cloudinary Image Link");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InternalImageUrl",
                table: "Recipes");

            migrationBuilder.RenameColumn(
                name: "ExternalImageUrl",
                table: "Recipes",
                newName: "ImageUrl");
        }
    }
}
