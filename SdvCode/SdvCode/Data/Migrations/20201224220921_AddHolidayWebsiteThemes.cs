// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddHolidayWebsiteThemes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HolidayThemes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 120, nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolidayThemes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HolidayIcons",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 120, nullable: false),
                    Url = table.Column<string>(nullable: false),
                    HolidayThemeId = table.Column<string>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolidayIcons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HolidayIcons_HolidayThemes_HolidayThemeId",
                        column: x => x.HolidayThemeId,
                        principalTable: "HolidayThemes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HolidayIcons_HolidayThemeId",
                table: "HolidayIcons",
                column: "HolidayThemeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HolidayIcons");

            migrationBuilder.DropTable(
                name: "HolidayThemes");
        }
    }
}