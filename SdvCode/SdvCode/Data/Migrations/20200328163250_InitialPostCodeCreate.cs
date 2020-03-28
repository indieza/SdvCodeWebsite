using Microsoft.EntityFrameworkCore.Migrations;

namespace SdvCode.Data.Migrations
{
    public partial class InitialPostCodeCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostCodeId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PostCodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(nullable: false),
                    City = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostCodes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PostCodeId",
                table: "AspNetUsers",
                column: "PostCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_PostCodes_PostCodeId",
                table: "AspNetUsers",
                column: "PostCodeId",
                principalTable: "PostCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_PostCodes_PostCodeId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "PostCodes");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PostCodeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PostCodeId",
                table: "AspNetUsers");
        }
    }
}
