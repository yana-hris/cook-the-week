using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookTheWeek.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMealPlanDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meals_MealPlans_MealPlanId",
                table: "Meals");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b16cd48c-3d1c-45b6-81fd-c4337fb19401", "AQAAAAIAAYagAAAAEOJyQIyr6FvgmOnb2YYiFAVPsTkmBjKvFAoSS4v8g64dnO3Vtajfb0IZ76kxp0IJ0w==", "e760a6c7-af42-46ee-8183-1a81107389b4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("ddbaeab3-10d6-4993-be38-59cd03967107"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6a3d9528-6681-49b3-9093-6de1670c0585", "AQAAAAIAAYagAAAAEHJVFwA6s+DRQGTKHKlcgYCCkxGtF4+lxy1uaehH6sKBU8ZMShqDSgOm+EZvBsC9Mw==", "f8cc3031-b7d2-4716-8557-20884b2c4dd8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e8ec0c24-2dd1-4a7a-aefc-b54bc9a8e403"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "901acd19-1ad5-4291-bd78-8066354d239f", "AQAAAAIAAYagAAAAEMzkxvl5NGrXmkTJTcoXxACBE2vXxc9x8sf3ym16emCkMZK6teVSRjzNzXxAGImkAw==", "34c8f872-3965-468f-993c-f67a8d8d7765" });

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

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c427131b-7393-4bfb-ab8f-1ad28c7f050e", "AQAAAAIAAYagAAAAEPG/MsRlwtbCY9C3JYgHuJF2glmQpW7ghxinDe5X4uz6iBmVv0/u9+aTnHSsI4uI+g==", "49f90f4d-35ab-4827-b41a-dadc86dce46d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("ddbaeab3-10d6-4993-be38-59cd03967107"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "994d2a01-cb34-46d2-924e-1a2bf830c6f7", "AQAAAAIAAYagAAAAEIgPPtgF7g7w5mBkVScywnmzJVjHPK8rSpLpQz4PXseOYK9VKOYcUfpD18zn61U/zg==", "0cf840a7-7ccf-471b-8606-ae08da832071" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e8ec0c24-2dd1-4a7a-aefc-b54bc9a8e403"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a76cfe66-e21d-4183-9473-688eaf74ae80", "AQAAAAIAAYagAAAAEPCH4KcwZjOnvcAFR3Ha1F40RPE1rIIPJOT3IbHfHU/Ms2dUI+YtkuVhMTRCo4mNhw==", "ae6aecd5-80ff-4be4-b806-628270b63ff7" });

            migrationBuilder.AddForeignKey(
                name: "FK_Meals_MealPlans_MealPlanId",
                table: "Meals",
                column: "MealPlanId",
                principalTable: "MealPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
