using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookTheWeek.Data.Migrations
{
    /// <inheritdoc />
    public partial class FicRecipeIngredientCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dbc00b96-9b47-41e3-b092-1dda5ec54dd9", "AQAAAAIAAYagAAAAEOf4kbbJMVSRd7n9pv+HOKQKhEUdgUXpAE72vrYyS4ZcCZEzkWKZ0jjMJhCbKzYMng==", "177b48d0-e76c-440a-9eb4-a1020cbb8446" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e8ec0c24-2dd1-4a7a-aefc-b54bc9a8e403"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b1262b19-4fe8-4b4d-a93f-2a5cd2d439df", "AQAAAAIAAYagAAAAEGDzDtwo41nWrviIQLwicKNbw3ZzMb0WoNURxMgf2EuSgcii4zLmjP3g8TRvh+u6hQ==", "e8aae201-cc0a-46d3-a7b3-e7f8fedc929d" });

            migrationBuilder.UpdateData(
                table: "IngredientCategories",
                keyColumn: "Id",
                keyValue: 10,
                column: "Name",
                value: "Fats and Oils, Sauces and Broths");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 54,
                column: "Name",
                value: "Paprika");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "pc/s");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "clove/s");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "tsp/s");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "tbsp/s");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 9,
                column: "Name",
                value: "cup/s");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 10,
                column: "Name",
                value: "bunch/es");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 11,
                column: "Name",
                value: "pkg/s");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 12,
                column: "Name",
                value: "slice/s");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 13,
                column: "Name",
                value: "pinch/es");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                table: "IngredientCategories",
                keyColumn: "Id",
                keyValue: 10,
                column: "Name",
                value: "Fats and Oils");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 54,
                column: "Name",
                value: "Paprika (Red Pepper)");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "piece(s)");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "clove(s)");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "tsp");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 9,
                column: "Name",
                value: "cup(s)");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 10,
                column: "Name",
                value: "bunch(es)");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 11,
                column: "Name",
                value: "pkg(s)");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 12,
                column: "Name",
                value: "slice(s)");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 13,
                column: "Name",
                value: "pinch(es)");
        }
    }
}
