// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class UpdateEmojisRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmojiSkins_EmojiId",
                table: "EmojiSkins");

            migrationBuilder.DropColumn(
                name: "EmojiSkinId",
                table: "Emojis");

            migrationBuilder.CreateIndex(
                name: "IX_EmojiSkins_EmojiId",
                table: "EmojiSkins",
                column: "EmojiId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmojiSkins_EmojiId",
                table: "EmojiSkins");

            migrationBuilder.AddColumn<string>(
                name: "EmojiSkinId",
                table: "Emojis",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmojiSkins_EmojiId",
                table: "EmojiSkins",
                column: "EmojiId",
                unique: true,
                filter: "[EmojiId] IS NOT NULL");
        }
    }
}