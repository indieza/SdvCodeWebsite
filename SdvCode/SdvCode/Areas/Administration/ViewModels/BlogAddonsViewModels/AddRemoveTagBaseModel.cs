// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.BlogAddonsViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Models.Blog;

    public class AddRemoveTagBaseModel
    {
        public ICollection<string> TagsNames { get; set; }

        public AddRemoveTagInputModel AddRemoveTagInputModel { get; set; }
    }
}