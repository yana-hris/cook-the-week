using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CookTheWeek.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixSeededData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 11, new Guid("1bec581c-d30c-4adf-ae77-7e7401b37b04") });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 54, new Guid("1bec581c-d30c-4adf-ae77-7e7401b37b04") });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 113, new Guid("1bec581c-d30c-4adf-ae77-7e7401b37b04") });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 118, new Guid("1bec581c-d30c-4adf-ae77-7e7401b37b04") });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 122, new Guid("1bec581c-d30c-4adf-ae77-7e7401b37b04") });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 154, new Guid("1bec581c-d30c-4adf-ae77-7e7401b37b04") });

            migrationBuilder.InsertData(
                table: "RecipesIngredients",
                columns: new[] { "IngredientId", "RecipeId", "MeasureId", "Qty", "SpecificationId" },
                values: new object[,]
                {
                    { 11, new Guid("377a3703-e719-42db-b013-19e5bd23892a"), 5, 500m, null },
                    { 54, new Guid("377a3703-e719-42db-b013-19e5bd23892a"), 8, 1m, null },
                    { 113, new Guid("377a3703-e719-42db-b013-19e5bd23892a"), 1, 2m, null },
                    { 118, new Guid("377a3703-e719-42db-b013-19e5bd23892a"), 6, 1m, null },
                    { 122, new Guid("377a3703-e719-42db-b013-19e5bd23892a"), 5, 250m, null },
                    { 154, new Guid("377a3703-e719-42db-b013-19e5bd23892a"), 8, 3m, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 11, new Guid("377a3703-e719-42db-b013-19e5bd23892a") });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 54, new Guid("377a3703-e719-42db-b013-19e5bd23892a") });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 113, new Guid("377a3703-e719-42db-b013-19e5bd23892a") });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 118, new Guid("377a3703-e719-42db-b013-19e5bd23892a") });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 122, new Guid("377a3703-e719-42db-b013-19e5bd23892a") });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 154, new Guid("377a3703-e719-42db-b013-19e5bd23892a") });

            migrationBuilder.InsertData(
                table: "RecipesIngredients",
                columns: new[] { "IngredientId", "RecipeId", "MeasureId", "Qty", "SpecificationId" },
                values: new object[,]
                {
                    { 11, new Guid("1bec581c-d30c-4adf-ae77-7e7401b37b04"), 5, 500m, null },
                    { 54, new Guid("1bec581c-d30c-4adf-ae77-7e7401b37b04"), 8, 1m, null },
                    { 113, new Guid("1bec581c-d30c-4adf-ae77-7e7401b37b04"), 1, 2m, null },
                    { 118, new Guid("1bec581c-d30c-4adf-ae77-7e7401b37b04"), 6, 1m, null },
                    { 122, new Guid("1bec581c-d30c-4adf-ae77-7e7401b37b04"), 5, 250m, null },
                    { 154, new Guid("1bec581c-d30c-4adf-ae77-7e7401b37b04"), 8, 3m, null }
                });
        }
    }
}
