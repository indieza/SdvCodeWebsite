﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Constraints
{
    public static class SuccessMessages
    {
        public const string SuccessfullyAddedUserInRole = "{0} is added in role \"{1}\" successfully.";

        public const string SuccessfullyAddedRole = "Success added \"{0}\" role.";

        public const string SuccessfullyRemoveActionByName = "Successfully removed all \"{0}\" actions.";

        public const string SuccessfullyRemoveAllActions = "Successfully removed all users actions ({0} users actions).";

        public const string SuccessfullyRemoveUserImages = "Successfully removed {0} profile and cover images.";

        public const string SuccessfullyRemoveAllUsersImages = "Successfully removed all users images ({0} users images).";

        public const string SuccessfullySyncFollowUnfollow = "Successfully synchronize Follow-Unfollow relations.";

        public const string SuccessfullyConfirmedPhoneNumber = "Successfully confirmed phone number. You are added in \"{0}\" role.";

        public const string SuccessfullyBlockedUser = "Successfully blocked user {0}.";

        public const string SuccessfullyUnblockedUser = "Successfully unblocked user {0}.";

        public const string SuccessfullyBlockedAllUsers = "Successfully blocked all users ({0} users) - except Administrators.";

        public const string SuccessfullyUnblockedAllUsers = "Successfully unblocked all users ({0} users).";

        public const string SuccessfullyRemoveUserRole = "User {0} was successfully removed from \"{1}\" role.";

        public const string SuccessfullyFollowedUser = "You successfully follow {0}.";

        public const string SuccessfullyUnfollowedUser = "You successfully unfollow {0}.";

        public const string SuccessfullyDeleteAllActivity = "You successfully delete all of your activity.";

        public const string SuccessfullyDeletedActivityById = "You successfully delete your single \"{0}\" activity.";
    }
}