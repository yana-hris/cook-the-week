using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookTheWeek.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultValueToIsCookedPropertyOfMealEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsCooked",
                table: "Meals",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Recipe is cooked or not",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Recipe is cooked or not");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsCooked",
                table: "Meals",
                type: "bit",
                nullable: false,
                comment: "Recipe is cooked or not",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Recipe is cooked or not");
        }
    }
}
