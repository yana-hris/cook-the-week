using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookTheWeek.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDifficultyLevelToRecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add the DifficultyLevel column to the Recipes table
            migrationBuilder.AddColumn<int>(
                name: "DifficultyLevel",
                table: "Recipes",
                type: "int",
                nullable: true); // Use nullable: false and add defaultValue if required
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the DifficultyLevel column from the Recipes table
            migrationBuilder.DropColumn(
                name: "DifficultyLevel",
                table: "Recipes");
        }


    }
}
