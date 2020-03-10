using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Constraints
{
    public static class ErrorMessages
    {
        public const string UserNotInRol = "{0} is not in role {1}.";

        public const string InvalidInputModel = "Unexpected error :( Maybe invalid Input Model.";

        public const string UserAlreadyInRole = "{0} is already in role {1}.";

        public const string RoleExist = "{0} role already exits.";

        public const string NoActionsByGivenName = "There is no more \"{0}\" actions for cleaning.";

        public const string NoActionsForRemoving = "There is no more users actions for removing";
    }
}