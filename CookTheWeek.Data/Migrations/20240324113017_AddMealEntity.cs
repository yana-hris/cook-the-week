using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookTheWeek.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMealEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "Specifications",
                comment: "The Specification of the Recipe Ingredient");

            migrationBuilder.AlterTable(
                name: "RecipesIngredients",
                comment: "The Ingredients in Recipe",
                oldComment: "Съставки към рецепти");

            migrationBuilder.AlterTable(
                name: "Recipes",
                comment: "Recipe");

            migrationBuilder.AlterTable(
                name: "RecipeCategories",
                comment: "Recipes Category");

            migrationBuilder.AlterTable(
                name: "Measures",
                comment: "The Measure of the Recipe Ingredient");

            migrationBuilder.AlterTable(
                name: "Ingredients",
                comment: "Ingredient");

            migrationBuilder.AlterTable(
                name: "IngredientCategories",
                comment: "Ingredients Category");

            migrationBuilder.AlterTable(
                name: "FavoriteRecipes",
                comment: "Users` Favourite Recipes");

            migrationBuilder.AlterTable(
                name: "AspNetUsers",
                comment: "The Application User");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Specifications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "Specification Description",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Specifications",
                type: "int",
                nullable: false,
                comment: "Specification Key Identifier",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "SpecificationId",
                table: "RecipesIngredients",
                type: "int",
                nullable: true,
                comment: "Specification Key Identifier",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Ключ за характеристика на съставката");

            migrationBuilder.AlterColumn<decimal>(
                name: "Qty",
                table: "RecipesIngredients",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                comment: "Quantity of the Ingredient in this Recipe",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldComment: "Количество на съставка в рецепта");

            migrationBuilder.AlterColumn<int>(
                name: "MeasureId",
                table: "RecipesIngredients",
                type: "int",
                nullable: false,
                comment: "Measure Key Identifier",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Мерна единица за съставка в рецепта");

            migrationBuilder.AlterColumn<int>(
                name: "IngredientId",
                table: "RecipesIngredients",
                type: "int",
                nullable: false,
                comment: "Key Identifier for Ingredient",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Ключ съставка");

            migrationBuilder.AlterColumn<Guid>(
                name: "RecipeId",
                table: "RecipesIngredients",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Key Identifier for Recipe",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Ключ рецепта");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "TotalTime",
                table: "Recipes",
                type: "time",
                nullable: false,
                comment: "Recipe Cooking Time",
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Recipes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                comment: "Recipe Title",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "Servings",
                table: "Recipes",
                type: "int",
                nullable: false,
                comment: "Recipe Serving Size",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeCategoryId",
                table: "Recipes",
                type: "int",
                nullable: false,
                comment: "Recipe Category Key Identifier",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Creator of the recipe",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Recipes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Soft Delete for Recipe",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Instructions",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Recipe Instructions",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Recipes",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                comment: "Recipe Image Link",
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)",
                oldMaxLength: 2048);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Recipes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "Recipe Description",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Recipes",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                comment: "Recipe Date and Time Creation",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Recipes",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Key Indetifier",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RecipeCategories",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "Recipe Category Name",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "RecipeCategories",
                type: "int",
                nullable: false,
                comment: "Key Identifier",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Measures",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "Measure Name",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Measures",
                type: "int",
                nullable: false,
                comment: "Key Identifier",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Ingredients",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "Ingredient Name",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "IngredientCategoryId",
                table: "Ingredients",
                type: "int",
                nullable: false,
                comment: "Ingredient Category Key Identifier",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Ingredients",
                type: "int",
                nullable: false,
                comment: "Key Identifier",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "IngredientCategories",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "Ingredient Category Name",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "IngredientCategories",
                type: "int",
                nullable: false,
                comment: "Key identifier",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<Guid>(
                name: "RecipeId",
                table: "FavoriteRecipes",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Recipe Key Identifier",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "FavoriteRecipes",
                type: "uniqueidentifier",
                nullable: false,
                comment: "User Key Identifier",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateTable(
                name: "Meals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Meal Key Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Recipe Key Identifier"),
                    ServingSize = table.Column<int>(type: "int", nullable: false, comment: "Meal Serving Size"),
                    CookDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Meal Cook Date"),
                    IsCooked = table.Column<bool>(type: "bit", nullable: false, comment: "Recipe is cooked or not")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meals_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "A Meal is a Recipe with user-defined Serving Size and Cook Date");

            migrationBuilder.CreateIndex(
                name: "IX_Meals_RecipeId",
                table: "Meals",
                column: "RecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Meals");

            migrationBuilder.AlterTable(
                name: "Specifications",
                oldComment: "The Specification of the Recipe Ingredient");

            migrationBuilder.AlterTable(
                name: "RecipesIngredients",
                comment: "Съставки към рецепти",
                oldComment: "The Ingredients in Recipe");

            migrationBuilder.AlterTable(
                name: "Recipes",
                oldComment: "Recipe");

            migrationBuilder.AlterTable(
                name: "RecipeCategories",
                oldComment: "Recipes Category");

            migrationBuilder.AlterTable(
                name: "Measures",
                oldComment: "The Measure of the Recipe Ingredient");

            migrationBuilder.AlterTable(
                name: "Ingredients",
                oldComment: "Ingredient");

            migrationBuilder.AlterTable(
                name: "IngredientCategories",
                oldComment: "Ingredients Category");

            migrationBuilder.AlterTable(
                name: "FavoriteRecipes",
                oldComment: "Users` Favourite Recipes");

            migrationBuilder.AlterTable(
                name: "AspNetUsers",
                oldComment: "The Application User");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Specifications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "Specification Description");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Specifications",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Specification Key Identifier")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "SpecificationId",
                table: "RecipesIngredients",
                type: "int",
                nullable: true,
                comment: "Ключ за характеристика на съставката",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Specification Key Identifier");

            migrationBuilder.AlterColumn<decimal>(
                name: "Qty",
                table: "RecipesIngredients",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                comment: "Количество на съставка в рецепта",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldComment: "Quantity of the Ingredient in this Recipe");

            migrationBuilder.AlterColumn<int>(
                name: "MeasureId",
                table: "RecipesIngredients",
                type: "int",
                nullable: false,
                comment: "Мерна единица за съставка в рецепта",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Measure Key Identifier");

            migrationBuilder.AlterColumn<int>(
                name: "IngredientId",
                table: "RecipesIngredients",
                type: "int",
                nullable: false,
                comment: "Ключ съставка",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Key Identifier for Ingredient");

            migrationBuilder.AlterColumn<Guid>(
                name: "RecipeId",
                table: "RecipesIngredients",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Ключ рецепта",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Key Identifier for Recipe");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "TotalTime",
                table: "Recipes",
                type: "time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldComment: "Recipe Cooking Time");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Recipes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldComment: "Recipe Title");

            migrationBuilder.AlterColumn<int>(
                name: "Servings",
                table: "Recipes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Recipe Serving Size");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeCategoryId",
                table: "Recipes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Recipe Category Key Identifier");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Creator of the recipe");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Recipes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Soft Delete for Recipe");

            migrationBuilder.AlterColumn<string>(
                name: "Instructions",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Recipe Instructions");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Recipes",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)",
                oldMaxLength: 2048,
                oldComment: "Recipe Image Link");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Recipes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldComment: "Recipe Description");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Recipes",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()",
                oldComment: "Recipe Date and Time Creation");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Recipes",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Key Indetifier");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RecipeCategories",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "Recipe Category Name");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "RecipeCategories",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Key Identifier")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Measures",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "Measure Name");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Measures",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Key Identifier")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Ingredients",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "Ingredient Name");

            migrationBuilder.AlterColumn<int>(
                name: "IngredientCategoryId",
                table: "Ingredients",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Ingredient Category Key Identifier");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Ingredients",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Key Identifier")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "IngredientCategories",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "Ingredient Category Name");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "IngredientCategories",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Key identifier")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<Guid>(
                name: "RecipeId",
                table: "FavoriteRecipes",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Recipe Key Identifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "FavoriteRecipes",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "User Key Identifier");
        }
    }
}
