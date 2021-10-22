// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class UpdateFollowUnfollowRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "FollowUnfollows",
                newName: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUnfollows_FollowerId",
                table: "FollowUnfollows",
                column: "FollowerId");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUnfollows_AspNetUsers_ApplicationUserId",
                table: "FollowUnfollows",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUnfollows_AspNetUsers_FollowerId",
                table: "FollowUnfollows",
                column: "FollowerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowUnfollows_AspNetUsers_ApplicationUserId",
                table: "FollowUnfollows");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowUnfollows_AspNetUsers_FollowerId",
                table: "FollowUnfollows");

            migrationBuilder.DropIndex(
                name: "IX_FollowUnfollows_FollowerId",
                table: "FollowUnfollows");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "FollowUnfollows",
                newName: "PersonId");
        }
    }
}