// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class UpdateDataModelConstraintsForUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "ChatImages",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReasonToBeBlocked",
                table: "AspNetUsers",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatImages_ApplicationUserId",
                table: "ChatImages",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatImages_AspNetUsers_ApplicationUserId",
                table: "ChatImages",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatImages_AspNetUsers_ApplicationUserId",
                table: "ChatImages");

            migrationBuilder.DropIndex(
                name: "IX_ChatImages_ApplicationUserId",
                table: "ChatImages");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "ChatImages");

            migrationBuilder.AlterColumn<string>(
                name: "ReasonToBeBlocked",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 300,
                oldNullable: true);
        }
    }
}