using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookTheWeek.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMealPlanOwnerIdToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                table: "Recipes",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Recipe Creator",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Recipe Creator");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "20c7606c-18d8-40ad-872c-e85beddde4e4", "AQAAAAIAAYagAAAAEAR0c6iG1NutYWbyCPiUyvUI8aaj5rHElKkxx72c6Phztwz/9G0isASlaunzmMO+vg==", "8951e365-2824-4ff9-8d46-4be8bd53266e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e8ec0c24-2dd1-4a7a-aefc-b54bc9a8e403"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "291ae3a1-76a8-40d8-8b07-d186d60942d1", "AQAAAAIAAYagAAAAEFwICC00u4Wt4dQ2mf58dkb3c2eHoTS/+B9Q6rHtIr0mzx7S+O0wT27IPB4I3HRDeg==", "60e4d1a3-00f1-4365-b31e-44237ccbb3a1" });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("11112341-30e4-473f-b93a-d0352b978a84"),
                column: "OwnerId",
                value: new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"));

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("115e248e-3165-425d-aec6-5dda97c99be4"),
                column: "OwnerId",
                value: new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"));

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"),
                column: "OwnerId",
                value: new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"));

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("25c6718c-b53b-4092-9454-d6999355f12d"),
                column: "OwnerId",
                value: new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"));

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("27664df3-cb8d-4ff6-a2cf-da0745a17531"),
                column: "OwnerId",
                value: new Guid("e8ec0c24-2dd1-4a7a-aefc-b54bc9a8e403"));

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("294c6abe-0072-427e-a1e8-355ba414fa5b"),
                column: "OwnerId",
                value: new Guid("e8ec0c24-2dd1-4a7a-aefc-b54bc9a8e403"));

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                column: "OwnerId",
                value: new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"));

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                column: "OwnerId",
                value: new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"));

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("cd9be7fb-c016-4246-ac36-411f6c3ece14"),
                column: "OwnerId",
                value: new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"));

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_OwnerId",
                table: "Recipes",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_AspNetUsers_OwnerId",
                table: "Recipes",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_AspNetUsers_OwnerId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_OwnerId",
                table: "Recipes");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Recipe Creator",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Recipe Creator");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "15775793-09cc-4118-8e6c-f9c74171ba27", "AQAAAAIAAYagAAAAELC8RaayaXqFhRIZFcE7FejuRvGVy3oehrvtnTWaCyHvIhfWm3aMBi4pMy5DxIvEeA==", "b5a75f2f-22ba-4829-a86d-1d517e429119" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e8ec0c24-2dd1-4a7a-aefc-b54bc9a8e403"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e654d84b-1ba9-48b3-bc1e-b66d4d399e15", "AQAAAAIAAYagAAAAENvPJD2sGd0BWOUbRVDfmeUPdcG+kW7BU9OLQlJCb0/FM0krujwq1L5XE9r7G39ltg==", "d4ec94e0-3b35-4648-8569-fa18ff444bef" });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("11112341-30e4-473f-b93a-d0352b978a84"),
                column: "OwnerId",
                value: "72ed6dd1-7c97-4af7-ab79-fc72e4a53b16");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("115e248e-3165-425d-aec6-5dda97c99be4"),
                column: "OwnerId",
                value: "72ed6dd1-7c97-4af7-ab79-fc72e4a53b16");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"),
                column: "OwnerId",
                value: "72ed6dd1-7c97-4af7-ab79-fc72e4a53b16");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("25c6718c-b53b-4092-9454-d6999355f12d"),
                column: "OwnerId",
                value: "72ed6dd1-7c97-4af7-ab79-fc72e4a53b16");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("27664df3-cb8d-4ff6-a2cf-da0745a17531"),
                column: "OwnerId",
                value: "e8ec0c24-2dd1-4a7a-aefc-b54bc9a8e403");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("294c6abe-0072-427e-a1e8-355ba414fa5b"),
                column: "OwnerId",
                value: "e8ec0c24-2dd1-4a7a-aefc-b54bc9a8e403");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                column: "OwnerId",
                value: "72ed6dd1-7c97-4af7-ab79-fc72e4a53b16");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                column: "OwnerId",
                value: "72ed6dd1-7c97-4af7-ab79-fc72e4a53b16");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("cd9be7fb-c016-4246-ac36-411f6c3ece14"),
                column: "OwnerId",
                value: "72ed6dd1-7c97-4af7-ab79-fc72e4a53b16");
        }
    }
}
