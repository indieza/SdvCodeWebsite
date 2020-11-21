// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class UpdateRelationshipsBetweenChatImagesAndMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChatMessageId",
                table: "ChatImages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ChatImages_ChatMessageId",
                table: "ChatImages",
                column: "ChatMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatImages_ChatMessages_ChatMessageId",
                table: "ChatImages",
                column: "ChatMessageId",
                principalTable: "ChatMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatImages_ChatMessages_ChatMessageId",
                table: "ChatImages");

            migrationBuilder.DropIndex(
                name: "IX_ChatImages_ChatMessageId",
                table: "ChatImages");

            migrationBuilder.DropColumn(
                name: "ChatMessageId",
                table: "ChatImages");
        }
    }
}