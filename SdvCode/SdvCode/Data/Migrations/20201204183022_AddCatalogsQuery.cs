// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddCatalogsQuery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Users Catalog
            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT CATALOG UsersCatalog AS DEFAULT;",
                suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT INDEX ON [AspNetUsers]([UserName], [FirstName], [LastName]) KEY INDEX PK_AspNetUsers;",
                suppressTransaction: true);

            // Posts Catalog
            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT CATALOG PostsCatalog AS DEFAULT;",
                suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT INDEX ON [Posts]([Title], [Content], [ShortContent]) KEY INDEX PK_Posts;",
                suppressTransaction: true);

            // Products Catalog
            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT CATALOG ProductsCatalog AS DEFAULT;",
                suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT INDEX ON [Products]([Description], [Name], [SpecificationsDescription]) KEY INDEX PK_Products;",
                suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}