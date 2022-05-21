﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Constraints
{
    public static class ErrorMessages
    {
        public const string UserNotInRole = "{0} is not in role {1}.";

        public const string InvalidInputModel = "Unexpected error :( Maybe invalid Input Model.";

        public const string UserAlreadyInRole = "{0} is already in role {1}.";

        public const string RoleExist = "{0} role already exits.";

        public const string NoActionsByGivenName = "There is no more \"{0}\" actions for cleaning.";

        public const string NoActionsForRemoving = "There is no more users actions for removing.";

        public const string NoUserImagesByGivenUsername = "{0} has no any images for deleting.";

        public const string NoMoreUsersImagesForRemoving = "There is no more users images for removing.";

        public const string NoDataForSyncFollowUnfollow = "There is no Follow-Unfollow relations for synchronize.";

        public const string UserAlreadyBlocked = "User {0} is already blocked or he is \"Administrator\". Administrators could not be blocked!!!";

        public const string UserAlreadyUnblocked = "User {0} is already unblocked.";

        public const string AllUsersAlreadyBlocked = "All users except Administrators are already blocked.";

        public const string AllUsersAlreadyUnblocked = "All users  are already unblocked.";

        public const string CategoryAlreadyExist = "\"{0}\" category already exist.";

        public const string TagAlreadyExist = "\"{0}\" tag already exist.";

        public const string YouAreBlock = "Bro you are blocked. Don't make bullshits :)";

        public const string NotInBlogRoles = "You're not in \"{0}\" role at least. Go to your account add phone number and verify it.";

        public const string NotExistingPost = "There is no such post!";

        public const string NotExistingComment = "There is no such comment!";

        public const string NoPermissionsToCreateBlogPost = "You don't have permissions to create post!";

        public const string NoPermissionToDeleteComment = "You don't have permissions to delete a comment!";

        public const string NoPermissionToEditComment = "You don't have permissions to edit a comment!";

        public const string NoPermissionToCreateComment = "You don't have permissions to create a comment!";

        public const string NoPermissionsToDeleteBlogPost = "You don't have permissions to delete post.";

        public const string NoPermissionToEditBlogPost = "You don't have permissions to edit post.";

        public const string NotApprovedBlogPost = "You cannot see not approved post.";

        public const string CannotLikeNotApprovedBlogPost = "You cannot like not approved post.";

        public const string CannotUnlikeNotApprovedBlogPost = "You cannot unlike not approved post.";

        public const string CannotAddToFavoriteNotApprovedBlogPost = "You cannot add to your favorite list not approved post.";

        public const string CannotRemoveFromFavoriteNotApprovedBlogPost = "You cannot remove from your favorite list not approved post.";

        public const string CannotEditBlogPost = "You cannot edit banned blog post.";

        public const string NotAbleToChat = "Your are not able to chat.";

        public const string DontMakeBullshits = "Bro are you a cheater. Don't make bullshits :)";

        public const string CannotCommentNotApprovedBlogPost = "You cannot comment not approved post.";

        public const string CannotCommentNotApprovedComment = "You cannot comment not approved comment.";

        public const string TagDoesNotExist = "Tag does not exist.";

        public const string ProductCategoryAlreadyExist = "\"{0}\" category already exist.";

        public const string ProductAlreadyExist = "\"{0}\" product already exist.";

        public const string EmojiAlreadyExist = "\"{0}\" emoji, already exist.";

        public const string EmojiDoesNotExist = "Emoji ID does not exist.";

        public const string ChatThemeAlreadyExist = "\"{0}\" chat theme name already exist.";

        public const string ChatThemeDoesNotAlreadyExist = "Chat theme ID does not exist.";

        public const string StickerTypeAlreadyExist = "\"{0}\" sticker type already exist.";

        public const string StickerTypeDoesNotExist = "Sticker type ID does not exist.";

        public const string StickerAlreadyExist = "\"{0}\" sticker already exist.";

        public const string StickerDoesNotExist = "Sticker ID does not exist.";

        public const string StickerAlreadyTypeExist = "\"{0}\" sticker type already exist.";

        public const string HolidayThemeAlreadyExist = "\"{0}\" holiday theme already exist.";

        public const string HolidayThemeDoesNotExist = "Holiday theme does not exist.";

        public const string CannotAddEmptyQuickReply = "You cannot add NULL or EMPTY quick chat reply.";

        public const string ChatQuickReplyDoesNotExist = "Chat quick reply does not exist.";
    }
}