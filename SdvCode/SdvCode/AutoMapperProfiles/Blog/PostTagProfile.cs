// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.AutoMapperProfiles.Blog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using SdvCode.Models.Blog;
    using SdvCode.ViewModels.Post.ViewModels;
    using SdvCode.ViewModels.Post.ViewModels.PostPage;
    using SdvCode.ViewModels.Tag;
    using SdvCode.ViewModels.Tag.TopTag;

    public class PostTagProfile : Profile
    {
        public PostTagProfile()
        {
            this.CreateMap<Tag, PostTagViewModel>();

            this.CreateMap<Tag, TopTagViewModel>()
                .ForMember(
                    dm => dm.Count,
                    mo => mo.MapFrom(x => x.TagsPosts.Count));
        }
    }
}