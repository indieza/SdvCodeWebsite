// Copyright (c) SDV Code Project. All rights reserved.
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

        public const string NotInBlogRoles = "You're not in \"{0}\" role. Go to your account add phone number and verify it.";

        public const string NotInDeletePostRoles = "You don't have permissions to delete post.";

        public const string NotInEditPostRoles = "You don't have permissions to edit post.";

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

        public const string TagDoesNotExist = "\"{0}\" tag does not exist.";

        public const string ProductCategoryAlreadyExist = "\"{0}\" category already exist.";

        public const string ProductAlreadyExist = "\"{0}\" product already exist.";
    }
}