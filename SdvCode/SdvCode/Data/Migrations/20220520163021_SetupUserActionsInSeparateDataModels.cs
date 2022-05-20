using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SdvCode.Migrations
{
    public partial class SetupUserActionsInSeparateDataModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserActions_ApplicationUsers_ApplicationUserId",
                table: "UserActions");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "UserActions");

            migrationBuilder.DropColumn(
                name: "ActionDate",
                table: "UserActions");

            migrationBuilder.DropColumn(
                name: "ActionStatus",
                table: "UserActions");

            migrationBuilder.DropColumn(
                name: "CoverImageUrl",
                table: "UserActions");

            migrationBuilder.DropColumn(
                name: "FollowerUsername",
                table: "UserActions");

            migrationBuilder.DropColumn(
                name: "PersonUsername",
                table: "UserActions");

            migrationBuilder.DropColumn(
                name: "PostContent",
                table: "UserActions");

            migrationBuilder.DropColumn(
                name: "PostTitle",
                table: "UserActions");

            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "UserActions");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "UserActions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "BaseUserActionId",
                table: "UserActions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "BaseUserActions",
                columns: table => new
                {
                    UserActionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActionStatus = table.Column<int>(type: "int", nullable: false),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    SystemMessage = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseUserActions", x => x.UserActionId);
                    table.ForeignKey(
                        name: "FK_BaseUserActions_ApplicationUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChangeCoverImageUserActions",
                columns: table => new
                {
                    UserActionId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeCoverImageUserActions", x => x.UserActionId);
                    table.ForeignKey(
                        name: "FK_ChangeCoverImageUserActions_BaseUserActions_UserActionId",
                        column: x => x.UserActionId,
                        principalTable: "BaseUserActions",
                        principalColumn: "UserActionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChangeProfilePictureUserActions",
                columns: table => new
                {
                    UserActionId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeProfilePictureUserActions", x => x.UserActionId);
                    table.ForeignKey(
                        name: "FK_ChangeProfilePictureUserActions_BaseUserActions_UserActionId",
                        column: x => x.UserActionId,
                        principalTable: "BaseUserActions",
                        principalColumn: "UserActionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CreatePostUserActions",
                columns: table => new
                {
                    UserActionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatePostUserActions", x => x.UserActionId);
                    table.ForeignKey(
                        name: "FK_CreatePostUserActions_BaseUserActions_UserActionId",
                        column: x => x.UserActionId,
                        principalTable: "BaseUserActions",
                        principalColumn: "UserActionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreatePostUserActions_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeletedPostUserActions",
                columns: table => new
                {
                    UserActionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ShortContent = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: false),
                    DeleterApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedPostUserActions", x => x.UserActionId);
                    table.ForeignKey(
                        name: "FK_DeletedPostUserActions_ApplicationUsers_DeleterApplicationUserId",
                        column: x => x.DeleterApplicationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeletedPostUserActions_BaseUserActions_UserActionId",
                        column: x => x.UserActionId,
                        principalTable: "BaseUserActions",
                        principalColumn: "UserActionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeleteOwnPostUserActions",
                columns: table => new
                {
                    UserActionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ShortContent = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeleteOwnPostUserActions", x => x.UserActionId);
                    table.ForeignKey(
                        name: "FK_DeleteOwnPostUserActions_BaseUserActions_UserActionId",
                        column: x => x.UserActionId,
                        principalTable: "BaseUserActions",
                        principalColumn: "UserActionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeletePostUserActions",
                columns: table => new
                {
                    UserActionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ShortContent = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletePostUserActions", x => x.UserActionId);
                    table.ForeignKey(
                        name: "FK_DeletePostUserActions_BaseUserActions_UserActionId",
                        column: x => x.UserActionId,
                        principalTable: "BaseUserActions",
                        principalColumn: "UserActionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EditedPostUserActions",
                columns: table => new
                {
                    UserActionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EditorApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EditedPostUserActions", x => x.UserActionId);
                    table.ForeignKey(
                        name: "FK_EditedPostUserActions_ApplicationUsers_EditorApplicationUserId",
                        column: x => x.EditorApplicationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EditedPostUserActions_BaseUserActions_UserActionId",
                        column: x => x.UserActionId,
                        principalTable: "BaseUserActions",
                        principalColumn: "UserActionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EditedPostUserActions_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EditOwnPostUserActions",
                columns: table => new
                {
                    UserActionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EditOwnPostUserActions", x => x.UserActionId);
                    table.ForeignKey(
                        name: "FK_EditOwnPostUserActions_BaseUserActions_UserActionId",
                        column: x => x.UserActionId,
                        principalTable: "BaseUserActions",
                        principalColumn: "UserActionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EditOwnPostUserActions_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EditPersonalDataUserActions",
                columns: table => new
                {
                    UserActionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EditPersonalDataUserActions", x => x.UserActionId);
                    table.ForeignKey(
                        name: "FK_EditPersonalDataUserActions_BaseUserActions_UserActionId",
                        column: x => x.UserActionId,
                        principalTable: "BaseUserActions",
                        principalColumn: "UserActionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EditPostUserActions",
                columns: table => new
                {
                    UserActionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EditPostUserActions", x => x.UserActionId);
                    table.ForeignKey(
                        name: "FK_EditPostUserActions_BaseUserActions_UserActionId",
                        column: x => x.UserActionId,
                        principalTable: "BaseUserActions",
                        principalColumn: "UserActionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EditPostUserActions_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FollowedUserActions",
                columns: table => new
                {
                    UserActionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FollowerApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowedUserActions", x => x.UserActionId);
                    table.ForeignKey(
                        name: "FK_FollowedUserActions_ApplicationUsers_FollowerApplicationUserId",
                        column: x => x.FollowerApplicationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FollowedUserActions_BaseUserActions_UserActionId",
                        column: x => x.UserActionId,
                        principalTable: "BaseUserActions",
                        principalColumn: "UserActionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FollowUserActions",
                columns: table => new
                {
                    UserActionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FollowingApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowUserActions", x => x.UserActionId);
                    table.ForeignKey(
                        name: "FK_FollowUserActions_ApplicationUsers_FollowingApplicationUserId",
                        column: x => x.FollowingApplicationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FollowUserActions_BaseUserActions_UserActionId",
                        column: x => x.UserActionId,
                        principalTable: "BaseUserActions",
                        principalColumn: "UserActionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LikedPostUserActions",
                columns: table => new
                {
                    UserActionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LikerApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikedPostUserActions", x => x.UserActionId);
                    table.ForeignKey(
                        name: "FK_LikedPostUserActions_ApplicationUsers_LikerApplicationUserId",
                        column: x => x.LikerApplicationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LikedPostUserActions_BaseUserActions_UserActionId",
                        column: x => x.UserActionId,
                        principalTable: "BaseUserActions",
                        principalColumn: "UserActionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LikedPostUserActions_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LikeOwnPostUserActions",
                columns: table => new
                {
                    UserActionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikeOwnPostUserActions", x => x.UserActionId);
                    table.ForeignKey(
                        name: "FK_LikeOwnPostUserActions_BaseUserActions_UserActionId",
                        column: x => x.UserActionId,
                        principalTable: "BaseUserActions",
                        principalColumn: "UserActionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LikeOwnPostUserActions_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LikePostUserActions",
                columns: table => new
                {
                    UserActionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikePostUserActions", x => x.UserActionId);
                    table.ForeignKey(
                        name: "FK_LikePostUserActions_BaseUserActions_UserActionId",
                        column: x => x.UserActionId,
                        principalTable: "BaseUserActions",
                        principalColumn: "UserActionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LikePostUserActions_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnfollowedUserActions",
                columns: table => new
                {
                    UserActionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UnfollowerApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnfollowedUserActions", x => x.UserActionId);
                    table.ForeignKey(
                        name: "FK_UnfollowedUserActions_ApplicationUsers_UnfollowerApplicationUserId",
                        column: x => x.UnfollowerApplicationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnfollowedUserActions_BaseUserActions_UserActionId",
                        column: x => x.UserActionId,
                        principalTable: "BaseUserActions",
                        principalColumn: "UserActionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UnfollowUserActions",
                columns: table => new
                {
                    UserActionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UnfollowingApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnfollowUserActions", x => x.UserActionId);
                    table.ForeignKey(
                        name: "FK_UnfollowUserActions_ApplicationUsers_UnfollowingApplicationUserId",
                        column: x => x.UnfollowingApplicationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnfollowUserActions_BaseUserActions_UserActionId",
                        column: x => x.UserActionId,
                        principalTable: "BaseUserActions",
                        principalColumn: "UserActionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UnlikedPostUserActions",
                columns: table => new
                {
                    UserActionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UnlikerApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnlikedPostUserActions", x => x.UserActionId);
                    table.ForeignKey(
                        name: "FK_UnlikedPostUserActions_ApplicationUsers_UnlikerApplicationUserId",
                        column: x => x.UnlikerApplicationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnlikedPostUserActions_BaseUserActions_UserActionId",
                        column: x => x.UserActionId,
                        principalTable: "BaseUserActions",
                        principalColumn: "UserActionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnlikedPostUserActions_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnlikeOwnPostUserActions",
                columns: table => new
                {
                    UserActionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnlikeOwnPostUserActions", x => x.UserActionId);
                    table.ForeignKey(
                        name: "FK_UnlikeOwnPostUserActions_BaseUserActions_UserActionId",
                        column: x => x.UserActionId,
                        principalTable: "BaseUserActions",
                        principalColumn: "UserActionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnlikeOwnPostUserActions_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnlikePostUserActions",
                columns: table => new
                {
                    UserActionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnlikePostUserActions", x => x.UserActionId);
                    table.ForeignKey(
                        name: "FK_UnlikePostUserActions_BaseUserActions_UserActionId",
                        column: x => x.UserActionId,
                        principalTable: "BaseUserActions",
                        principalColumn: "UserActionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnlikePostUserActions_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserActions_BaseUserActionId",
                table: "UserActions",
                column: "BaseUserActionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BaseUserActions_ApplicationUserId",
                table: "BaseUserActions",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatePostUserActions_PostId",
                table: "CreatePostUserActions",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_DeletedPostUserActions_DeleterApplicationUserId",
                table: "DeletedPostUserActions",
                column: "DeleterApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EditedPostUserActions_EditorApplicationUserId",
                table: "EditedPostUserActions",
                column: "EditorApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EditedPostUserActions_PostId",
                table: "EditedPostUserActions",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_EditOwnPostUserActions_PostId",
                table: "EditOwnPostUserActions",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_EditPostUserActions_PostId",
                table: "EditPostUserActions",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowedUserActions_FollowerApplicationUserId",
                table: "FollowedUserActions",
                column: "FollowerApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUserActions_FollowingApplicationUserId",
                table: "FollowUserActions",
                column: "FollowingApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LikedPostUserActions_LikerApplicationUserId",
                table: "LikedPostUserActions",
                column: "LikerApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LikedPostUserActions_PostId",
                table: "LikedPostUserActions",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_LikeOwnPostUserActions_PostId",
                table: "LikeOwnPostUserActions",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_LikePostUserActions_PostId",
                table: "LikePostUserActions",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_UnfollowedUserActions_UnfollowerApplicationUserId",
                table: "UnfollowedUserActions",
                column: "UnfollowerApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UnfollowUserActions_UnfollowingApplicationUserId",
                table: "UnfollowUserActions",
                column: "UnfollowingApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UnlikedPostUserActions_PostId",
                table: "UnlikedPostUserActions",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_UnlikedPostUserActions_UnlikerApplicationUserId",
                table: "UnlikedPostUserActions",
                column: "UnlikerApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UnlikeOwnPostUserActions_PostId",
                table: "UnlikeOwnPostUserActions",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_UnlikePostUserActions_PostId",
                table: "UnlikePostUserActions",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserActions_ApplicationUsers_ApplicationUserId",
                table: "UserActions",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserActions_BaseUserActions_BaseUserActionId",
                table: "UserActions",
                column: "BaseUserActionId",
                principalTable: "BaseUserActions",
                principalColumn: "UserActionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserActions_ApplicationUsers_ApplicationUserId",
                table: "UserActions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserActions_BaseUserActions_BaseUserActionId",
                table: "UserActions");

            migrationBuilder.DropTable(
                name: "ChangeCoverImageUserActions");

            migrationBuilder.DropTable(
                name: "ChangeProfilePictureUserActions");

            migrationBuilder.DropTable(
                name: "CreatePostUserActions");

            migrationBuilder.DropTable(
                name: "DeletedPostUserActions");

            migrationBuilder.DropTable(
                name: "DeleteOwnPostUserActions");

            migrationBuilder.DropTable(
                name: "DeletePostUserActions");

            migrationBuilder.DropTable(
                name: "EditedPostUserActions");

            migrationBuilder.DropTable(
                name: "EditOwnPostUserActions");

            migrationBuilder.DropTable(
                name: "EditPersonalDataUserActions");

            migrationBuilder.DropTable(
                name: "EditPostUserActions");

            migrationBuilder.DropTable(
                name: "FollowedUserActions");

            migrationBuilder.DropTable(
                name: "FollowUserActions");

            migrationBuilder.DropTable(
                name: "LikedPostUserActions");

            migrationBuilder.DropTable(
                name: "LikeOwnPostUserActions");

            migrationBuilder.DropTable(
                name: "LikePostUserActions");

            migrationBuilder.DropTable(
                name: "UnfollowedUserActions");

            migrationBuilder.DropTable(
                name: "UnfollowUserActions");

            migrationBuilder.DropTable(
                name: "UnlikedPostUserActions");

            migrationBuilder.DropTable(
                name: "UnlikeOwnPostUserActions");

            migrationBuilder.DropTable(
                name: "UnlikePostUserActions");

            migrationBuilder.DropTable(
                name: "BaseUserActions");

            migrationBuilder.DropIndex(
                name: "IX_UserActions_BaseUserActionId",
                table: "UserActions");

            migrationBuilder.DropColumn(
                name: "BaseUserActionId",
                table: "UserActions");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "UserActions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Action",
                table: "UserActions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActionDate",
                table: "UserActions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ActionStatus",
                table: "UserActions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CoverImageUrl",
                table: "UserActions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FollowerUsername",
                table: "UserActions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonUsername",
                table: "UserActions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostContent",
                table: "UserActions",
                type: "nvarchar(350)",
                maxLength: 350,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostTitle",
                table: "UserActions",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "UserActions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserActions_ApplicationUsers_ApplicationUserId",
                table: "UserActions",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
