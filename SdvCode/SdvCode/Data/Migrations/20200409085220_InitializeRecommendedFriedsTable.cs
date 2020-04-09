// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class InitializeRecommendedFriedsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecommendedFriends",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecommendedUsername = table.Column<string>(nullable: false),
                    RecommendedFirstName = table.Column<string>(maxLength: 15, nullable: true),
                    RecommendedLastName = table.Column<string>(maxLength: 15, nullable: true),
                    RecommendedImageUrl = table.Column<string>(nullable: false),
                    RecommendedCoverImage = table.Column<string>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommendedFriends", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecommendedFriends_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecommendedFriends_ApplicationUserId",
                table: "RecommendedFriends",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecommendedFriends");
        }
    }
}