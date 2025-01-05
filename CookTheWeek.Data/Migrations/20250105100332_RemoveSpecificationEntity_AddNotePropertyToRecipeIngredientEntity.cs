using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookTheWeek.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSpecificationEntity_AddNotePropertyToRecipeIngredientEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "RecipesIngredients",
                type: "nvarchar(max)",
                nullable: true,
                comment: "User Notes for the given RecipeIngredient");

            // Migrate data from Specifications to Note column in RecipesIngredients
            migrationBuilder.Sql(@"
                UPDATE RecipesIngredients
                SET Note = (
                    SELECT Description
                    FROM Specifications
                    WHERE Specifications.Id = RecipesIngredients.SpecificationId
                )
                WHERE SpecificationId IS NOT NULL
            ");


            migrationBuilder.DropForeignKey(
                name: "FK_RecipesIngredients_Specifications_SpecificationId",
                table: "RecipesIngredients");

            migrationBuilder.DropTable(
                name: "Specifications");

            migrationBuilder.DropIndex(
                name: "IX_RecipesIngredients_RecipeId_IngredientId_MeasureId",
                table: "RecipesIngredients");

            migrationBuilder.DropIndex(
                name: "IX_RecipesIngredients_RecipeId_IngredientId_MeasureId_SpecificationId",
                table: "RecipesIngredients");

            migrationBuilder.DropIndex(
                name: "IX_RecipesIngredients_SpecificationId",
                table: "RecipesIngredients");

            migrationBuilder.DropColumn(
                name: "SpecificationId",
                table: "RecipesIngredients");
            

            migrationBuilder.CreateIndex(
                name: "IX_RecipesIngredients_RecipeId_IngredientId_MeasureId",
                table: "RecipesIngredients",
                columns: new[] { "RecipeId", "IngredientId", "MeasureId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RecipesIngredients_RecipeId_IngredientId_MeasureId",
                table: "RecipesIngredients");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "RecipesIngredients");

            migrationBuilder.AddColumn<int>(
                name: "SpecificationId",
                table: "RecipesIngredients",
                type: "int",
                nullable: true,
                comment: "Key identifier for Specification");

            migrationBuilder.CreateTable(
                name: "Specifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Key Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Specification Description")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specifications", x => x.Id);
                },
                comment: "Specification of Recipe-Ingredient");

            migrationBuilder.CreateIndex(
                name: "IX_RecipesIngredients_RecipeId_IngredientId_MeasureId",
                table: "RecipesIngredients",
                columns: new[] { "RecipeId", "IngredientId", "MeasureId" },
                unique: true,
                filter: "[SpecificationId] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RecipesIngredients_RecipeId_IngredientId_MeasureId_SpecificationId",
                table: "RecipesIngredients",
                columns: new[] { "RecipeId", "IngredientId", "MeasureId", "SpecificationId" },
                unique: true,
                filter: "[SpecificationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RecipesIngredients_SpecificationId",
                table: "RecipesIngredients",
                column: "SpecificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipesIngredients_Specifications_SpecificationId",
                table: "RecipesIngredients",
                column: "SpecificationId",
                principalTable: "Specifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
