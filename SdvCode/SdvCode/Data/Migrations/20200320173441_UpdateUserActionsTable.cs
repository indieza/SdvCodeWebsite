// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class UpdateUserActionsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FollowerProfileImageUrl",
                table: "UserActions");

            migrationBuilder.DropColumn(
                name: "PersonProfileImageUrl",
                table: "UserActions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FollowerProfileImageUrl",
                table: "UserActions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonProfileImageUrl",
                table: "UserActions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}