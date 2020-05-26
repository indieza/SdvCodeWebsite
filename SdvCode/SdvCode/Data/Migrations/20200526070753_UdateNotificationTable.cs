// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class UdateNotificationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "UserNotifications",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "UserNotifications",
                maxLength: 500,
                nullable: false,
                defaultValue: string.Empty);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserNotifications");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "UserNotifications");
        }
    }
}