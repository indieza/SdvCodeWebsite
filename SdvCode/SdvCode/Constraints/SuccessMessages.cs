using System;
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
    }
}