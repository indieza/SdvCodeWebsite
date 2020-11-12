// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddChatThemesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChatThemeId",
                table: "Groups",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChatThemes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    Url = table.Column<string>(nullable: false),
                    GroupId = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatThemes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatThemes_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatThemes_GroupId",
                table: "ChatThemes",
                column: "GroupId",
                unique: true,
                filter: "[GroupId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatThemes");

            migrationBuilder.DropColumn(
                name: "ChatThemeId",
                table: "Groups");
        }
    }
}