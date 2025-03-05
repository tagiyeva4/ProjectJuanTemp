using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniAppJuanTemplate.Data.Migrations
{
    /// <inheritdoc />
    public partial class BugFixProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rate",
                table: "Products",
                newName: "Rate");

            migrationBuilder.RenameColumn(
                name: "Rate",
                table: "ProductComments",
                newName: "Rate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rate",
                table: "Products",
                newName: "Rating");

            migrationBuilder.RenameColumn(
                name: "Rate",
                table: "ProductComments",
                newName: "Rating");
        }
    }
}
