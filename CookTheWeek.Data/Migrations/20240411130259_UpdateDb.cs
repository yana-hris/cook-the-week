using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookTheWeek.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_IngredientCategories_IngredientCategoryId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_RecipeCategories_RecipeCategoryId",
                table: "Recipes");

            migrationBuilder.RenameColumn(
                name: "RecipeCategoryId",
                table: "Recipes",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipes_RecipeCategoryId",
                table: "Recipes",
                newName: "IX_Recipes_CategoryId");

            migrationBuilder.RenameColumn(
                name: "IngredientCategoryId",
                table: "Ingredients",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredients_IngredientCategoryId",
                table: "Ingredients",
                newName: "IX_Ingredients_CategoryId");

            migrationBuilder.AlterTable(
                name: "Specifications",
                comment: "Specification of Recipe-Ingredient",
                oldComment: "The Specification of the Recipe Ingredient");

            migrationBuilder.AlterTable(
                name: "RecipesIngredients",
                comment: "Recipe Ingredient",
                oldComment: "The Ingredients in Recipe");

            migrationBuilder.AlterTable(
                name: "Measures",
                comment: "Measure of Recipe-Ingredient",
                oldComment: "The Measure of the Recipe Ingredient");

            migrationBuilder.AlterTable(
                name: "Meals",
                comment: "Meal",
                oldComment: "A Meal is a Recipe with user-defined Serving Size and Cook Date");

            migrationBuilder.AlterTable(
                name: "MealPlans",
                comment: "Meal Plan",
                oldComment: "Meal Plan belongs to a user and consists of user-defined Meals");

            migrationBuilder.AlterTable(
                name: "IngredientCategories",
                comment: "Ingredient Category",
                oldComment: "Ingredients Category");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Specifications",
                type: "int",
                nullable: false,
                comment: "Key Identifier",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Specification Key Identifier")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<decimal>(
                name: "Qty",
                table: "RecipesIngredients",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                comment: "Quantity of Ingredient in Recipe",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldComment: "Quantity of the Ingredient in this Recipe");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Recipe Creator",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Creator of the recipe");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Recipes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Soft Delete for a Recipe",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Soft Delete for Recipe");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Recipes",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                comment: "Date and Time of a Recipe Creation",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()",
                oldComment: "Recipe Date and Time Creation");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCooked",
                table: "Meals",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Meal completion Identifier",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Recipe is cooked or not");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MealPlans",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "Meal Plan Name",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<bool>(
                name: "IsFinished",
                table: "MealPlans",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Meal Plan Completion Identifier",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Is the Meal Plan already finished or not");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "83ee9d6e-9cea-4040-828a-678f9bb3636e", "AQAAAAIAAYagAAAAEKu74TdwoaMUW8mACAuqZYLnkRy88dtdnyQC3xW/bEw7sVXQMn4ycxXfNiUPTkpt1Q==", "09259197-4a5e-492b-9c0c-8c374a274a4c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e8ec0c24-2dd1-4a7a-aefc-b54bc9a8e403"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f9ab2450-dc9e-4627-a653-e60b0ac3dc33", "AQAAAAIAAYagAAAAEFMCrFBZ5jF0DyTz098FR3M9FO8zxt0+kElaQO1OWUWmDp5f4bmOnvBQyHSceMEM3Q==", "3700fcc3-19b7-4f19-81b6-cacb4bfba8ad" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 10,
                column: "Name",
                value: "Wild Game Meat");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 11,
                column: "Name",
                value: "Ground Meat");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 22,
                column: "Name",
                value: "Goat Cheese");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 27,
                column: "Name",
                value: "Cottage Cheese");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 35,
                column: "Name",
                value: "Green Peas");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 36,
                column: "Name",
                value: "White Beans");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 38,
                column: "Name",
                value: "Mung Beans");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 39,
                column: "Name",
                value: "Red Lentils");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 40,
                column: "Name",
                value: "Black Beans");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 42,
                column: "Name",
                value: "Green Lentils");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 43,
                column: "Name",
                value: "White Flour");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 44,
                column: "Name",
                value: "Whole Wheat Flour");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 45,
                column: "Name",
                value: "Rye Flour");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 46,
                column: "Name",
                value: "Spelt Flour");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 47,
                column: "Name",
                value: "Almond Flour");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 48,
                column: "Name",
                value: "Pine Nuts");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 49,
                column: "Name",
                value: "Spelled Flour");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 50,
                column: "Name",
                value: "Corn Flour");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 51,
                column: "Name",
                value: "Rice Flour");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 53,
                column: "Name",
                value: "Black Pepper");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 54,
                column: "Name",
                value: "Paprika (Red Pepper)");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 61,
                column: "Name",
                value: "Curry Powder");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 74,
                column: "Name",
                value: "Wild Garlic");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 75,
                column: "Name",
                value: "Fennel (dill)");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 77,
                column: "Name",
                value: "Clary Sage");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 82,
                column: "Name",
                value: "Maple Syrup");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 83,
                column: "Name",
                value: "Coconut Sugar");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 84,
                column: "Name",
                value: "Coconut Sugar");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 85,
                column: "Name",
                value: "Powdered Sugar");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 86,
                column: "Name",
                value: "Brown Sugar");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 88,
                column: "Name",
                value: "Agave Syrup");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 89,
                column: "Name",
                value: "Cane Sugar");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 100,
                column: "Name",
                value: "Brussels Sprout");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 116,
                column: "Name",
                value: "Ginger Root");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 119,
                column: "Name",
                value: "Sweet Potato(s)");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 120,
                column: "Name",
                value: "Sweet Corn");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 123,
                column: "Name",
                value: "Cherry Tomatoe(s)");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 127,
                column: "Name",
                value: "Red Bell Pepper");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 128,
                column: "Name",
                value: "Green Bell Pepper");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 129,
                column: "Name",
                value: "Red Onion(s)");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 151,
                column: "Name",
                value: "Sour Cherries");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 154,
                column: "Name",
                value: "Sunflower Oil");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 155,
                column: "Name",
                value: "Olive Oil");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 158,
                column: "Name",
                value: "Coconut Oil");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 159,
                column: "Name",
                value: "Avocado Oil");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 163,
                column: "Name",
                value: "Sour Cream");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 164,
                column: "Name",
                value: "Whipped Cream");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 165,
                column: "Name",
                value: "Coconut Cream");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 166,
                column: "Name",
                value: "Diary Free Cream");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 167,
                column: "Name",
                value: "Sesame Oil");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 175,
                column: "Name",
                value: "Skip Jack");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 178,
                column: "Name",
                value: "Silver Catfish (Pangasius)");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 182,
                column: "Name",
                value: "Salmon Trout");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 185,
                column: "Name",
                value: "Lemon Juice");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 186,
                column: "Name",
                value: "Tomatoe Paste");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 190,
                column: "Name",
                value: "Brown Rice");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 191,
                column: "Name",
                value: "Black Rice");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 198,
                column: "Name",
                value: "Chia Seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 206,
                column: "Name",
                value: "Brazil Nuts");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 207,
                column: "Name",
                value: "Cashew Nuts");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 208,
                column: "Name",
                value: "Hazel Nuts");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 214,
                column: "Name",
                value: "Pumpkin Seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 215,
                column: "Name",
                value: "Flax Seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 216,
                column: "Name",
                value: "Sesame Seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 217,
                column: "Name",
                value: "Poppy Seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 218,
                column: "Name",
                value: "Sunflower Seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 219,
                column: "Name",
                value: "Psyllium Seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 222,
                column: "Name",
                value: "Vegetable Broth");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 223,
                column: "Name",
                value: "Beef Broth");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 224,
                column: "Name",
                value: "Fish Broth");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 225,
                column: "Name",
                value: "Fish Broth");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 242,
                column: "Name",
                value: "Sourdough Bread");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 243,
                column: "Name",
                value: "Full Grain Bread");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 244,
                column: "Name",
                value: "White Bread");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 245,
                column: "Name",
                value: "Rye Bread");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 251,
                column: "Name",
                value: "Hemp Seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 252,
                column: "Name",
                value: "Himalayan Salt");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("115e248e-3165-425d-aec6-5dda97c99be4"),
                column: "Title",
                value: "Fruity Strawberry Smoothy");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"),
                column: "Title",
                value: "Avocado Toast");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("25c6718c-b53b-4092-9454-d6999355f12d"),
                column: "Title",
                value: "Homemade Chicken Soup");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                column: "Title",
                value: "Beef Stew");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("cd9be7fb-c016-4246-ac36-411f6c3ece14"),
                column: "Title",
                value: "Overnight Oats (prepare the night beofre)");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_IngredientCategories_CategoryId",
                table: "Ingredients",
                column: "CategoryId",
                principalTable: "IngredientCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_RecipeCategories_CategoryId",
                table: "Recipes",
                column: "CategoryId",
                principalTable: "RecipeCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_IngredientCategories_CategoryId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_RecipeCategories_CategoryId",
                table: "Recipes");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Recipes",
                newName: "RecipeCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipes_CategoryId",
                table: "Recipes",
                newName: "IX_Recipes_RecipeCategoryId");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Ingredients",
                newName: "IngredientCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredients_CategoryId",
                table: "Ingredients",
                newName: "IX_Ingredients_IngredientCategoryId");

            migrationBuilder.AlterTable(
                name: "Specifications",
                comment: "The Specification of the Recipe Ingredient",
                oldComment: "Specification of Recipe-Ingredient");

            migrationBuilder.AlterTable(
                name: "RecipesIngredients",
                comment: "The Ingredients in Recipe",
                oldComment: "Recipe Ingredient");

            migrationBuilder.AlterTable(
                name: "Measures",
                comment: "The Measure of the Recipe Ingredient",
                oldComment: "Measure of Recipe-Ingredient");

            migrationBuilder.AlterTable(
                name: "Meals",
                comment: "A Meal is a Recipe with user-defined Serving Size and Cook Date",
                oldComment: "Meal");

            migrationBuilder.AlterTable(
                name: "MealPlans",
                comment: "Meal Plan belongs to a user and consists of user-defined Meals",
                oldComment: "Meal Plan");

            migrationBuilder.AlterTable(
                name: "IngredientCategories",
                comment: "Ingredients Category",
                oldComment: "Ingredient Category");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Specifications",
                type: "int",
                nullable: false,
                comment: "Specification Key Identifier",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Key Identifier")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

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
                oldComment: "Quantity of Ingredient in Recipe");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Creator of the recipe",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Recipe Creator");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Recipes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Soft Delete for Recipe",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Soft Delete for a Recipe");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Recipes",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                comment: "Recipe Date and Time Creation",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()",
                oldComment: "Date and Time of a Recipe Creation");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCooked",
                table: "Meals",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Recipe is cooked or not",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Meal completion Identifier");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MealPlans",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "Meal Plan Name");

            migrationBuilder.AlterColumn<bool>(
                name: "IsFinished",
                table: "MealPlans",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Is the Meal Plan already finished or not",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Meal Plan Completion Identifier");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "de59dff2-8b1c-468f-9224-1805c1bcd339", "AQAAAAIAAYagAAAAEKIN1ax9aHDD3bhkNHaU2j/yG+LwKzYEaaOZWGwgUWV/kcMwZPHidMklFGbSfAWk+w==", "9791ebb4-6f06-4151-ae15-d4d06898b0fa" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e8ec0c24-2dd1-4a7a-aefc-b54bc9a8e403"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1a2ca4be-09d1-4827-b0db-49fd6e153c9b", "AQAAAAIAAYagAAAAEFO+ThX1QTgukg4Vi4z5OBXSFhSo7m/s3GwffbThHagR29hMr4/hCgLxB6m4LfVTGA==", "393dfa2a-46ac-43df-8201-16957b98949c" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 10,
                column: "Name",
                value: "Wild game meat");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 11,
                column: "Name",
                value: "Ground meat");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 22,
                column: "Name",
                value: "Goat cheese");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 27,
                column: "Name",
                value: "Cottage cheese");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 35,
                column: "Name",
                value: "Green peas");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 36,
                column: "Name",
                value: "White beans");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 38,
                column: "Name",
                value: "Mung beans");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 39,
                column: "Name",
                value: "Red lentils");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 40,
                column: "Name",
                value: "Black beans");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 42,
                column: "Name",
                value: "Green lentils");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 43,
                column: "Name",
                value: "White flour");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 44,
                column: "Name",
                value: "Whole wheat flour");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 45,
                column: "Name",
                value: "Rye flour");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 46,
                column: "Name",
                value: "Spelt flour");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 47,
                column: "Name",
                value: "Almond flour");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 48,
                column: "Name",
                value: "Pine nuts");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 49,
                column: "Name",
                value: "Spelled flour");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 50,
                column: "Name",
                value: "Corn flour");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 51,
                column: "Name",
                value: "Rice flour");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 53,
                column: "Name",
                value: "Black pepper");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 54,
                column: "Name",
                value: "Paprika(pepper)");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 61,
                column: "Name",
                value: "Curry powder");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 74,
                column: "Name",
                value: "Wild garlic");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 75,
                column: "Name",
                value: "Fennel(");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 77,
                column: "Name",
                value: "Clary sage");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 82,
                column: "Name",
                value: "Maple syrup");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 83,
                column: "Name",
                value: "Coconut sugar");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 84,
                column: "Name",
                value: "Coconut sugar");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 85,
                column: "Name",
                value: "Powdered sugar");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 86,
                column: "Name",
                value: "Brown sugar");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 88,
                column: "Name",
                value: "Agave syrup");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 89,
                column: "Name",
                value: "Cane sugar");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 100,
                column: "Name",
                value: "Brussels sprout");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 116,
                column: "Name",
                value: "Ginger root");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 119,
                column: "Name",
                value: "Sweet potato(s)");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 120,
                column: "Name",
                value: "Sweet corn");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 123,
                column: "Name",
                value: "Cherry tomatoe(s)");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 127,
                column: "Name",
                value: "Red bell pepper");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 128,
                column: "Name",
                value: "Green bell pepper");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 129,
                column: "Name",
                value: "Red onion(s)");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 151,
                column: "Name",
                value: "Sour cherries");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 154,
                column: "Name",
                value: "Sunflower oil");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 155,
                column: "Name",
                value: "Olive oil");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 158,
                column: "Name",
                value: "Coconut oil");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 159,
                column: "Name",
                value: "Avocado oil");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 163,
                column: "Name",
                value: "Sour cream");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 164,
                column: "Name",
                value: "Whipped cream");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 165,
                column: "Name",
                value: "Coconut cream");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 166,
                column: "Name",
                value: "Diary free cream");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 167,
                column: "Name",
                value: "Sesame oil");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 175,
                column: "Name",
                value: "Skip jack");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 178,
                column: "Name",
                value: "Silver catfish(");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 182,
                column: "Name",
                value: "Salmon trout");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 185,
                column: "Name",
                value: "Lemon juice");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 186,
                column: "Name",
                value: "Tomatoe paste");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 190,
                column: "Name",
                value: "Brown rice");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 191,
                column: "Name",
                value: "Black rice");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 198,
                column: "Name",
                value: "Chia seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 206,
                column: "Name",
                value: "Brazil nuts");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 207,
                column: "Name",
                value: "Cashew nuts");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 208,
                column: "Name",
                value: "Hazel nuts");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 214,
                column: "Name",
                value: "Pumpkin seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 215,
                column: "Name",
                value: "Flax seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 216,
                column: "Name",
                value: "Sesame seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 217,
                column: "Name",
                value: "Poppy seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 218,
                column: "Name",
                value: "Sunflower seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 219,
                column: "Name",
                value: "Psyllium seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 222,
                column: "Name",
                value: "Vegetable broth");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 223,
                column: "Name",
                value: "Beef broth");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 224,
                column: "Name",
                value: "Fish broth");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 225,
                column: "Name",
                value: "Fish broth");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 242,
                column: "Name",
                value: "Sourdough bread");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 243,
                column: "Name",
                value: "Full grain bread");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 244,
                column: "Name",
                value: "White bread");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 245,
                column: "Name",
                value: "Rye bread");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 251,
                column: "Name",
                value: "Hemp seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 252,
                column: "Name",
                value: "Himalayan salt");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("115e248e-3165-425d-aec6-5dda97c99be4"),
                column: "Title",
                value: "Fruity strawberry smoothy");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"),
                column: "Title",
                value: "Avocado toast");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("25c6718c-b53b-4092-9454-d6999355f12d"),
                column: "Title",
                value: "Homemade chicken soup");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                column: "Title",
                value: "Beef stew");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("cd9be7fb-c016-4246-ac36-411f6c3ece14"),
                column: "Title",
                value: "Overnight oats(the night beofre)");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_IngredientCategories_IngredientCategoryId",
                table: "Ingredients",
                column: "IngredientCategoryId",
                principalTable: "IngredientCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_RecipeCategories_RecipeCategoryId",
                table: "Recipes",
                column: "RecipeCategoryId",
                principalTable: "RecipeCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
