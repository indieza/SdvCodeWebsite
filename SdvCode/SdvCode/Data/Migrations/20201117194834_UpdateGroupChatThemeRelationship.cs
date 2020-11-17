// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class UpdateGroupChatThemeRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatThemes_Groups_GroupId",
                table: "ChatThemes");

            migrationBuilder.DropIndex(
                name: "IX_ChatThemes_GroupId",
                table: "ChatThemes");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "ChatThemes");

            migrationBuilder.AlterColumn<string>(
                name: "ChatThemeId",
                table: "Groups",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ChatThemeId",
                table: "Groups",
                column: "ChatThemeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_ChatThemes_ChatThemeId",
                table: "Groups",
                column: "ChatThemeId",
                principalTable: "ChatThemes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_ChatThemes_ChatThemeId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_ChatThemeId",
                table: "Groups");

            migrationBuilder.AlterColumn<string>(
                name: "ChatThemeId",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "ChatThemes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatThemes_GroupId",
                table: "ChatThemes",
                column: "GroupId",
                unique: true,
                filter: "[GroupId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatThemes_Groups_GroupId",
                table: "ChatThemes",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}