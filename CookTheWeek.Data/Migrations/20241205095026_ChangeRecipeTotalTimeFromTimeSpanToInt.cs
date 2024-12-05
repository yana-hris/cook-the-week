using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookTheWeek.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRecipeTotalTimeFromTimeSpanToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "TotalTime",
            //    table: "Recipes");

            migrationBuilder.AddColumn<int>(
                name: "TotalTimeMinutes",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Recipe Cooking Time");

            migrationBuilder.Sql(@"
            UPDATE Recipes
            SET TotalTimeMinutes = DATEDIFF(MINUTE, 0, TotalTime)
            WHERE TotalTime IS NOT NULL
        ");

            // Step 3: Drop the old TotalTime column
            migrationBuilder.DropColumn(
                name: "TotalTime",
                table: "Recipes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TotalTime",
                table: "Recipes",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                comment: "Recipe Cooking Time");

            // Step 2: Migrate data back from TotalTimeMinutes to TotalTime
            migrationBuilder.Sql(@"
            UPDATE Recipes
            SET TotalTime = DATEADD(MINUTE, TotalTimeMinutes, 0)
            WHERE TotalTimeMinutes IS NOT NULL
        ");

            migrationBuilder.DropColumn(
               name: "TotalTimeMinutes",
               table: "Recipes");
        }
    }
}
