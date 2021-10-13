// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.AutoMapperProfiles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using SdvCode.Models.Blog;
    using SdvCode.ViewModels.Post.ViewModels;

    public class PostImageProfile : Profile
    {
        public PostImageProfile()
        {
            this.CreateMap<PostImage, PostPostImageViewModel>();
        }
    }
}