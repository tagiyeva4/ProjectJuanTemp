using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiniAppJuanTemplate.Data.Migrations
{
    /// <inheritdoc />
    public partial class addConnectionIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7c2697b0-84c1-45a2-8850-8ab262c00a69");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8a6f10ea-11fe-46af-a4b3-ef0d9270cafc");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "dd5a2e4a-da86-4cd9-98bc-4b99fd2a3185", "c5a86f31-1b43-4805-81ad-f3cf0ff672ce" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dd5a2e4a-da86-4cd9-98bc-4b99fd2a3185");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5a86f31-1b43-4805-81ad-f3cf0ff672ce");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7c2697b0-84c1-45a2-8850-8ab262c00a69", null, "Admin", "ADMIN" },
                    { "8a6f10ea-11fe-46af-a4b3-ef0d9270cafc", null, "SuperAdmin", "SUPERADMIN" },
                    { "dd5a2e4a-da86-4cd9-98bc-4b99fd2a3185", null, "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "c5a86f31-1b43-4805-81ad-f3cf0ff672ce", 0, "a18e058e-7660-4628-a4f6-a7d12daaf289", null, false, "Test", false, null, null, "_Test", "AQAAAAIAAYagAAAAEPfig1Vz4dGy3A8ByhAKAiTEa5hyr+b9UzRMsfQJSdm9xtz6O+tnO1IIMyMqnsGXmg==", null, false, "09ecafc5-5a12-4622-afc2-ffab1c1db1ab", false, "_test" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "dd5a2e4a-da86-4cd9-98bc-4b99fd2a3185", "c5a86f31-1b43-4805-81ad-f3cf0ff672ce" });
        }
    }
}
