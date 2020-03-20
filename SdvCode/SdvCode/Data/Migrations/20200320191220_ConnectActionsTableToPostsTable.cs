// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ConnectActionsTableToPostsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PostId",
                table: "UserActions",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserActions_PostId",
                table: "UserActions",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserActions_Posts_PostId",
                table: "UserActions",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserActions_Posts_PostId",
                table: "UserActions");

            migrationBuilder.DropIndex(
                name: "IX_UserActions_PostId",
                table: "UserActions");

            migrationBuilder.AlterColumn<string>(
                name: "PostId",
                table: "UserActions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}