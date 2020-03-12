// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.DashboardViewModels
{
    public class DashboardIndexViewModel
    {
        public DashboardViewModel DashboardViewModel { get; set; }

        public CreateRoleInputModel CreateRole { get; set; }

        public AddUserInRoleInputModel AddUserInRole { get; set; }

        public RemoveUserFromRoleInputModel RemoveUserFromRole { get; set; }
    }
}