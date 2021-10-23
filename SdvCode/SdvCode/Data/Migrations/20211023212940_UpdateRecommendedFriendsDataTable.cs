// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class UpdateRecommendedFriendsDataTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecommendedFriends_AspNetUsers_ApplicationUserId",
                table: "RecommendedFriends");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecommendedFriends",
                table: "RecommendedFriends");

            migrationBuilder.DropIndex(
                name: "IX_RecommendedFriends_ApplicationUserId",
                table: "RecommendedFriends");

            migrationBuilder.DropColumn(
                name: "RecommendedCoverImage",
                table: "RecommendedFriends");

            migrationBuilder.DropColumn(
                name: "RecommendedFirstName",
                table: "RecommendedFriends");

            migrationBuilder.DropColumn(
                name: "RecommendedImageUrl",
                table: "RecommendedFriends");

            migrationBuilder.DropColumn(
                name: "RecommendedLastName",
                table: "RecommendedFriends");

            migrationBuilder.DropColumn(
                name: "RecommendedUsername",
                table: "RecommendedFriends");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "RecommendedFriends",
                newName: "RecommendedApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecommendedFriends",
                table: "RecommendedFriends",
                columns: new[] { "ApplicationUserId", "RecommendedApplicationUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_RecommendedFriends_RecommendedApplicationUserId",
                table: "RecommendedFriends",
                column: "RecommendedApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendedFriends_AspNetUsers_ApplicationUserId",
                table: "RecommendedFriends",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendedFriends_AspNetUsers_RecommendedApplicationUserId",
                table: "RecommendedFriends",
                column: "RecommendedApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecommendedFriends_AspNetUsers_ApplicationUserId",
                table: "RecommendedFriends");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendedFriends_AspNetUsers_RecommendedApplicationUserId",
                table: "RecommendedFriends");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecommendedFriends",
                table: "RecommendedFriends");

            migrationBuilder.DropIndex(
                name: "IX_RecommendedFriends_RecommendedApplicationUserId",
                table: "RecommendedFriends");

            migrationBuilder.RenameColumn(
                name: "RecommendedApplicationUserId",
                table: "RecommendedFriends",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "RecommendedCoverImage",
                table: "RecommendedFriends",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "RecommendedFirstName",
                table: "RecommendedFriends",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecommendedImageUrl",
                table: "RecommendedFriends",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "RecommendedLastName",
                table: "RecommendedFriends",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecommendedUsername",
                table: "RecommendedFriends",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecommendedFriends",
                table: "RecommendedFriends",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendedFriends_ApplicationUserId",
                table: "RecommendedFriends",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendedFriends_AspNetUsers_ApplicationUserId",
                table: "RecommendedFriends",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}