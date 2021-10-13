// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Tag
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SdvCode.DataViewModels.Blog;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;

    public interface ITagService
    {
        Task<Tag> ExtractTagById(string id);

        Task<ICollection<PostViewModel>> ExtractPostsByTagId(string id, ApplicationUser user);
    }
}