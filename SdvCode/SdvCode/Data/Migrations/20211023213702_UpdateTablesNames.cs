// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class UpdateTablesNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cities_CityId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Countries_CountryId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_CountryCodes_CountryCodeId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_States_StateId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ZipCodes_ZipCodeId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_BlockedPosts_AspNetUsers_ApplicationUserId",
                table: "BlockedPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatImages_AspNetUsers_ApplicationUserId",
                table: "ChatImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_AspNetUsers_ApplicationUserId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_ApplicationUserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_FavouritePosts_AspNetUsers_ApplicationUserId",
                table: "FavouritePosts");

            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteStickers_AspNetUsers_ApplicationUserId",
                table: "FavouriteStickers");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowUnfollows_AspNetUsers_ApplicationUserId",
                table: "FollowUnfollows");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowUnfollows_AspNetUsers_FollowerId",
                table: "FollowUnfollows");

            migrationBuilder.DropForeignKey(
                name: "FK_PendingPosts_AspNetUsers_ApplicationUserId",
                table: "PendingPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_ApplicationUserId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_PostsLikes_AspNetUsers_UserId",
                table: "PostsLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductComments_AspNetUsers_ApplicationUserId",
                table: "ProductComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_AspNetUsers_ApplicationUserId",
                table: "ProductReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_QuickChatReplies_AspNetUsers_ApplicationUserId",
                table: "QuickChatReplies");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendedFriends_AspNetUsers_ApplicationUserId",
                table: "RecommendedFriends");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendedFriends_AspNetUsers_RecommendedApplicationUserId",
                table: "RecommendedFriends");

            migrationBuilder.DropForeignKey(
                name: "FK_UserActions_AspNetUsers_ApplicationUserId",
                table: "UserActions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotifications_AspNetUsers_ApplicationUserId",
                table: "UserNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersGroups_AspNetUsers_ApplicationUserId",
                table: "UsersGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "ApplicationUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "ApplicationUsersRoles");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "ApplicationRoles");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_ZipCodeId",
                table: "ApplicationUsers",
                newName: "IX_ApplicationUsers_ZipCodeId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_StateId",
                table: "ApplicationUsers",
                newName: "IX_ApplicationUsers_StateId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_CountryId",
                table: "ApplicationUsers",
                newName: "IX_ApplicationUsers_CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_CountryCodeId",
                table: "ApplicationUsers",
                newName: "IX_ApplicationUsers_CountryCodeId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_CityId",
                table: "ApplicationUsers",
                newName: "IX_ApplicationUsers_CityId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "ApplicationUsersRoles",
                newName: "IX_ApplicationUsersRoles_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUsers",
                table: "ApplicationUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUsersRoles",
                table: "ApplicationUsersRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationRoles",
                table: "ApplicationRoles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_Cities_CityId",
                table: "ApplicationUsers",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_Countries_CountryId",
                table: "ApplicationUsers",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_CountryCodes_CountryCodeId",
                table: "ApplicationUsers",
                column: "CountryCodeId",
                principalTable: "CountryCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_States_StateId",
                table: "ApplicationUsers",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_ZipCodes_ZipCodeId",
                table: "ApplicationUsers",
                column: "ZipCodeId",
                principalTable: "ZipCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsersRoles_ApplicationRoles_RoleId",
                table: "ApplicationUsersRoles",
                column: "RoleId",
                principalTable: "ApplicationRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsersRoles_ApplicationUsers_UserId",
                table: "ApplicationUsersRoles",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_ApplicationRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "ApplicationRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_ApplicationUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_ApplicationUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_ApplicationUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlockedPosts_ApplicationUsers_ApplicationUserId",
                table: "BlockedPosts",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatImages_ApplicationUsers_ApplicationUserId",
                table: "ChatImages",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_ApplicationUsers_ApplicationUserId",
                table: "ChatMessages",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_ApplicationUsers_ApplicationUserId",
                table: "Comments",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavouritePosts_ApplicationUsers_ApplicationUserId",
                table: "FavouritePosts",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteStickers_ApplicationUsers_ApplicationUserId",
                table: "FavouriteStickers",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUnfollows_ApplicationUsers_ApplicationUserId",
                table: "FollowUnfollows",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUnfollows_ApplicationUsers_FollowerId",
                table: "FollowUnfollows",
                column: "FollowerId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PendingPosts_ApplicationUsers_ApplicationUserId",
                table: "PendingPosts",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_ApplicationUsers_ApplicationUserId",
                table: "Posts",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostsLikes_ApplicationUsers_UserId",
                table: "PostsLikes",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComments_ApplicationUsers_ApplicationUserId",
                table: "ProductComments",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_ApplicationUsers_ApplicationUserId",
                table: "ProductReviews",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuickChatReplies_ApplicationUsers_ApplicationUserId",
                table: "QuickChatReplies",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendedFriends_ApplicationUsers_ApplicationUserId",
                table: "RecommendedFriends",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendedFriends_ApplicationUsers_RecommendedApplicationUserId",
                table: "RecommendedFriends",
                column: "RecommendedApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserActions_ApplicationUsers_ApplicationUserId",
                table: "UserActions",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotifications_ApplicationUsers_ApplicationUserId",
                table: "UserNotifications",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersGroups_ApplicationUsers_ApplicationUserId",
                table: "UsersGroups",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_Cities_CityId",
                table: "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_Countries_CountryId",
                table: "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_CountryCodes_CountryCodeId",
                table: "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_States_StateId",
                table: "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_ZipCodes_ZipCodeId",
                table: "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsersRoles_ApplicationRoles_RoleId",
                table: "ApplicationUsersRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsersRoles_ApplicationUsers_UserId",
                table: "ApplicationUsersRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_ApplicationRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_ApplicationUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_ApplicationUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_ApplicationUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_BlockedPosts_ApplicationUsers_ApplicationUserId",
                table: "BlockedPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatImages_ApplicationUsers_ApplicationUserId",
                table: "ChatImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_ApplicationUsers_ApplicationUserId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_ApplicationUsers_ApplicationUserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_FavouritePosts_ApplicationUsers_ApplicationUserId",
                table: "FavouritePosts");

            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteStickers_ApplicationUsers_ApplicationUserId",
                table: "FavouriteStickers");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowUnfollows_ApplicationUsers_ApplicationUserId",
                table: "FollowUnfollows");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowUnfollows_ApplicationUsers_FollowerId",
                table: "FollowUnfollows");

            migrationBuilder.DropForeignKey(
                name: "FK_PendingPosts_ApplicationUsers_ApplicationUserId",
                table: "PendingPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_ApplicationUsers_ApplicationUserId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_PostsLikes_ApplicationUsers_UserId",
                table: "PostsLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductComments_ApplicationUsers_ApplicationUserId",
                table: "ProductComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_ApplicationUsers_ApplicationUserId",
                table: "ProductReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_QuickChatReplies_ApplicationUsers_ApplicationUserId",
                table: "QuickChatReplies");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendedFriends_ApplicationUsers_ApplicationUserId",
                table: "RecommendedFriends");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendedFriends_ApplicationUsers_RecommendedApplicationUserId",
                table: "RecommendedFriends");

            migrationBuilder.DropForeignKey(
                name: "FK_UserActions_ApplicationUsers_ApplicationUserId",
                table: "UserActions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotifications_ApplicationUsers_ApplicationUserId",
                table: "UserNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersGroups_ApplicationUsers_ApplicationUserId",
                table: "UsersGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUsersRoles",
                table: "ApplicationUsersRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUsers",
                table: "ApplicationUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationRoles",
                table: "ApplicationRoles");

            migrationBuilder.RenameTable(
                name: "ApplicationUsersRoles",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "ApplicationUsers",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "ApplicationRoles",
                newName: "AspNetRoles");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUsersRoles_RoleId",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUsers_ZipCodeId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_ZipCodeId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUsers_StateId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_StateId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUsers_CountryId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUsers_CountryCodeId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_CountryCodeId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUsers_CityId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_CityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Cities_CityId",
                table: "AspNetUsers",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Countries_CountryId",
                table: "AspNetUsers",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_CountryCodes_CountryCodeId",
                table: "AspNetUsers",
                column: "CountryCodeId",
                principalTable: "CountryCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_States_StateId",
                table: "AspNetUsers",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ZipCodes_ZipCodeId",
                table: "AspNetUsers",
                column: "ZipCodeId",
                principalTable: "ZipCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlockedPosts_AspNetUsers_ApplicationUserId",
                table: "BlockedPosts",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatImages_AspNetUsers_ApplicationUserId",
                table: "ChatImages",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_AspNetUsers_ApplicationUserId",
                table: "ChatMessages",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_ApplicationUserId",
                table: "Comments",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavouritePosts_AspNetUsers_ApplicationUserId",
                table: "FavouritePosts",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteStickers_AspNetUsers_ApplicationUserId",
                table: "FavouriteStickers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_PendingPosts_AspNetUsers_ApplicationUserId",
                table: "PendingPosts",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_ApplicationUserId",
                table: "Posts",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostsLikes_AspNetUsers_UserId",
                table: "PostsLikes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComments_AspNetUsers_ApplicationUserId",
                table: "ProductComments",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_AspNetUsers_ApplicationUserId",
                table: "ProductReviews",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuickChatReplies_AspNetUsers_ApplicationUserId",
                table: "QuickChatReplies",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

            migrationBuilder.AddForeignKey(
                name: "FK_UserActions_AspNetUsers_ApplicationUserId",
                table: "UserActions",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotifications_AspNetUsers_ApplicationUserId",
                table: "UserNotifications",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersGroups_AspNetUsers_ApplicationUserId",
                table: "UsersGroups",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}