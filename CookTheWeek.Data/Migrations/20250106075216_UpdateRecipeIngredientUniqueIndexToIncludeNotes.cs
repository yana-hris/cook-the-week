using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookTheWeek.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRecipeIngredientUniqueIndexToIncludeNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RecipesIngredients_RecipeId_IngredientId_MeasureId",
                table: "RecipesIngredients");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "RecipesIngredients",
                type: "nvarchar(450)",
                nullable: true,
                comment: "User Notes for the given RecipeIngredient",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "User Notes for the given RecipeIngredient");

            migrationBuilder.CreateIndex(
                name: "IX_RecipesIngredients_RecipeId_IngredientId_MeasureId_Note",
                table: "RecipesIngredients",
                columns: new[] { "RecipeId", "IngredientId", "MeasureId", "Note" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RecipesIngredients_RecipeId_IngredientId_MeasureId_Note",
                table: "RecipesIngredients");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "RecipesIngredients",
                type: "nvarchar(max)",
                nullable: true,
                comment: "User Notes for the given RecipeIngredient",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true,
                oldComment: "User Notes for the given RecipeIngredient");

            migrationBuilder.CreateIndex(
                name: "IX_RecipesIngredients_RecipeId_IngredientId_MeasureId",
                table: "RecipesIngredients",
                columns: new[] { "RecipeId", "IngredientId", "MeasureId" },
                unique: true);
        }
    }
}
