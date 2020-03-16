// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddPostColums : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostContent",
                table: "UserActions",
                maxLength: 350,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostTitle",
                table: "UserActions",
                maxLength: 150,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostContent",
                table: "UserActions");

            migrationBuilder.DropColumn(
                name: "PostTitle",
                table: "UserActions");
        }
    }
}