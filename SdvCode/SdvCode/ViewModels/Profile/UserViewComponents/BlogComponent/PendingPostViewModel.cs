// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Profile.UserViewComponents.BlogComponent
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using SdvCode.Models.Blog;

    public class PendingPostViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string ShortContent { get; set; }

        public DateTime CreatedOn { get; set; }

        public BlogComponentCategoryViewModel Category { get; set; }

        public BlogComponentApplicationUserViewModel ApplicationUser { get; set; }
    }
}