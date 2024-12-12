using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookTheWeek.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRatingsEntityAndRecipeViewsAndAverageRatingProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                table: "Recipes",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                comment: "Recipe Average Rating calculated");

            migrationBuilder.AddColumn<int>(
                name: "Views",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Recipe Total Views");

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Recipe Key Identifier"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "User Key Identifier"),
                    RatingValue = table.Column<int>(type: "int", nullable: false, comment: "User Rating for a given Recipe from 1 to 5"),
                    RatingText = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "User Comment for a given Recipe explaining Rativng Value"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "Time of Rating Creation"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Soft Delete the Rating when the Recipe is deleted")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => new { x.UserId, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_Ratings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ratings_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Recipe Rating by a given User");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_RecipeId",
                table: "Ratings",
                column: "RecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Views",
                table: "Recipes");
        }
    }
}
