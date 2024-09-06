using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookTheWeek.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLockoutSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c475a28e-d106-4482-a675-1efccb795c4e", "AQAAAAIAAYagAAAAEC0/H3fy7Kfx3oCT/4ajxzPYm6jmylcZWlsRTByKPASc3td8HYndFYMXDC7zh/1Vdw==", "29bdf111-2dfe-46c0-8759-d0bf6a9a5534" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("ddbaeab3-10d6-4993-be38-59cd03967107"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "edd130ab-b184-495b-ae10-4229c471d858", "AQAAAAIAAYagAAAAEMow8LH9PsQEsxCyPDKzq9nF2FPAqmJh/DkM5ggL6IvcGhOvyU/ZLSFi2dPYYK6eYQ==", "1d631dc4-52ce-4fb4-8e93-3afabf95a76e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e8ec0c24-2dd1-4a7a-aefc-b54bc9a8e403"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "57db29f6-e5bb-469e-8475-29a92a91046e", "AQAAAAIAAYagAAAAEL0JKA2+/tr8IrTclM4vCiByfFWjw0/chjFbJvvlFHjc/MBKp5d1oeGkrQx8TgrFGA==", "287e51ec-b5c2-4c2e-ac4f-8f31423baa26" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "999e2e41-1570-46d9-b4ba-1da29c190551", "AQAAAAIAAYagAAAAEEnVfLHDyP105Fd7fvH5uYNeCLcFNEUIcGAUyNQbsBx9QlpKtyAmBgEVJTezig6JWw==", "9559bba5-a6b8-4a28-84a9-66cb103fdbb8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("ddbaeab3-10d6-4993-be38-59cd03967107"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "69feb5cf-0b2f-4d31-ba6b-ee77b2c3043e", "AQAAAAIAAYagAAAAEG1Xz0DR1KIUTCJnUmtGrEhNFfgIXmYmskmbYHkczMwx0AX/nfUjPfUAsL87GTwjqQ==", "e3d319d6-bc1f-40ee-846e-2e053577945d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e8ec0c24-2dd1-4a7a-aefc-b54bc9a8e403"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f96c398c-eee3-44e4-ae62-378a33b30c06", "AQAAAAIAAYagAAAAEGh6RHM/YL3vI52p4iD8tZuPlwkb2MQ+dN9SE8j/uw6/N8uovgGTVrLPkTUvYmi5bQ==", "b085410c-5950-490a-8183-1bbc667b80cb" });
        }
    }
}
