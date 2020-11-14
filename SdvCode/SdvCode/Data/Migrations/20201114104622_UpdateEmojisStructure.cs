// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class UpdateEmojisStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Emojis");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Emojis",
                maxLength: 120,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);

            migrationBuilder.AddColumn<string>(
                name: "EmojiSkinId",
                table: "Emojis",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Emojis",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "EmojiSkins",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 120, nullable: false),
                    Url = table.Column<string>(nullable: false),
                    Position = table.Column<int>(nullable: false),
                    EmojiId = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmojiSkins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmojiSkins_Emojis_EmojiId",
                        column: x => x.EmojiId,
                        principalTable: "Emojis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmojiSkins_EmojiId",
                table: "EmojiSkins",
                column: "EmojiId",
                unique: true,
                filter: "[EmojiId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmojiSkins");

            migrationBuilder.DropColumn(
                name: "EmojiSkinId",
                table: "Emojis");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Emojis");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Emojis",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 120);

            migrationBuilder.AddColumn<int>(
                name: "Code",
                table: "Emojis",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}