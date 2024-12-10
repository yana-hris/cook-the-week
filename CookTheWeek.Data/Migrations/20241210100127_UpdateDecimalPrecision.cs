using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookTheWeek.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDecimalPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Qty",
                table: "RecipesIngredients",
                type: "decimal(10,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                comment: "Quantity of Ingredient in Recipe",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldComment: "Quantity of Ingredient in Recipe");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Qty",
                table: "RecipesIngredients",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                comment: "Quantity of Ingredient in Recipe",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldComment: "Quantity of Ingredient in Recipe");
        }
    }
}
