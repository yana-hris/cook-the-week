using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CookTheWeek.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedRecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Recipes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Recipes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 2, 23, 10, 6, 34, 74, DateTimeKind.Utc).AddTicks(2596));

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "Id", "IngredientCategoryId", "Name" },
                values: new object[] { 189, 5, "Savory" });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "Id", "ApplicationUserId", "Description", "ImageUrl", "Instructions", "RecipeCategoryId", "Servings", "Title", "TotalTime" },
                values: new object[] { new Guid("1bec581c-d30c-4adf-ae77-7e7401b37b04"), null, "Мусака! Царицата на българската кухня! Едно от най-обичаните и най-често приготвяните ястия. Мусаката е абсолютният любимец както на всеки българин, така и на чужденците. Ако се притеснявате какво да сготвите на гостите от чужбина, можете да заложите на мусаката. Чуждестранните гости едва ли ще харесат таратора и едва ли ще съберат сили да пробват печените чушки с доматен сос, но мусаката задължително ще спечели доверието им, ще си оближат пръстите и ще искат порция допълнително. Рецептите за мусака нямат много вариации, вариантите са основно в приготвянето на заливката. Предложената тук рецепта за мусака е класическа, лесна и подходяща и за по-неопитните готвачи.", "https://www.supichka.com/files/images/1242/fit_1400_933.jpg", "Загрявате мазнината в дълбок тиган или тенджера. В загрятата мазнина задушете измитият, изчистен и нарязан на дребно лук, докато омекне. Добавяте нарязаните на дребно половината домати и каймата, черният пипер и чубрицата, както и една чаена лъжица сол. Може да ползвате кайма, каквато имате под ръка, но най-вкусната мусаката става с кайма смес – 50% свинско и 50 % телешко месо.\r\nСлед като водата от доматите изври добавяте червения пипер. Измивате, почиствате, обелвате и нарязвате на дребни кубчета картофите и ги прибавяте към месото. Намазнявате тавичка на дъното на която поставяте другата половина от доматите и върху тях изсипвате сместа за мусака. Добавяте една непълна чаена чаша гореща вода и поставяте в предварително загрята на 200 градуса фурна. Печете до докато водата изври, а картофите станат златисти.\r\nИдва моментът за приготвяне на заливката за мусаката. Тя става лесно и много бързо. Разбърквате киселото мляко и яйцата, след което при постоянно бъркане добавяте и брашното, до получаване на гладка смес. Накрая добавяте и настъргания на дребно кашкавал. Заливате мусаката и печете докато порозовее.", 4, 8, "Мусака", new TimeSpan(0, 2, 0, 0, 0) });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 189);

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

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("1bec581c-d30c-4adf-ae77-7e7401b37b04"));

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Recipes");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Recipes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);
        }
    }
}
