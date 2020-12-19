// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddFavouriteStickersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavouriteStickers",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(nullable: false),
                    StickerTypeId = table.Column<string>(nullable: false),
                    IsFavourite = table.Column<bool>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavouriteStickers", x => new { x.ApplicationUserId, x.StickerTypeId });
                    table.ForeignKey(
                        name: "FK_FavouriteStickers_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavouriteStickers_StickerTypes_StickerTypeId",
                        column: x => x.StickerTypeId,
                        principalTable: "StickerTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteStickers_StickerTypeId",
                table: "FavouriteStickers",
                column: "StickerTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavouriteStickers");
        }
    }
}