using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AYShop.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class BrandPic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MainImage",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainImage",
                table: "Brands");
        }
    }
}
