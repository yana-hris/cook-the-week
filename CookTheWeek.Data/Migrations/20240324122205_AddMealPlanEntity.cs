using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookTheWeek.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMealPlanEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MealPlanId",
                table: "Meals",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Meal Plan Key Identifier");

            migrationBuilder.CreateTable(
                name: "MealPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Meal Plan Key Identifier"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Meal Plan Owner Key Identifier"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()", comment: "Meal Plan Start Date"),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Is the Meal Plan already finished or not")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealPlans_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Meal Plan belongs to a user and consists of user-defined Meals");

            migrationBuilder.CreateIndex(
                name: "IX_Meals_MealPlanId",
                table: "Meals",
                column: "MealPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_MealPlans_OwnerId",
                table: "MealPlans",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meals_MealPlans_MealPlanId",
                table: "Meals",
                column: "MealPlanId",
                principalTable: "MealPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meals_MealPlans_MealPlanId",
                table: "Meals");

            migrationBuilder.DropTable(
                name: "MealPlans");

            migrationBuilder.DropIndex(
                name: "IX_Meals_MealPlanId",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "MealPlanId",
                table: "Meals");
        }
    }
}
