// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.DbUsageViewModels.DeleteActivities
{
    using System.ComponentModel.DataAnnotations;

    public class DeleteActivitiesByNameInputModel
    {
        [Required]
        [Display(Name = "Activity Name")]
        public string ActivityName { get; set; }
    }
}