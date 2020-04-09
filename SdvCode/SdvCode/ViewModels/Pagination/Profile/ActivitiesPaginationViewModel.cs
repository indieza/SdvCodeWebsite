// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Pagination.Profile
{
    using System.Collections.Generic;
    using SdvCode.ViewModels.Profile;

    public class ActivitiesPaginationViewModel
    {
        public string Username { get; set; }

        public IEnumerable<ActivitiesViewModel> Activities { get; set; } = new HashSet<ActivitiesViewModel>();
    }
}