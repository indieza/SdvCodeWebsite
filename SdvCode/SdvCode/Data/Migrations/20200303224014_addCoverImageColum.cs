using Microsoft.EntityFrameworkCore.Migrations;

namespace SdvCode.Data.Migrations
{
    public partial class addCoverImageColum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverImageUrl",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImageUrl",
                table: "AspNetUsers");
        }
    }
}