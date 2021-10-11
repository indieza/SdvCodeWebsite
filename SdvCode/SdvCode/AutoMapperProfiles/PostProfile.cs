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
    using SdvCode.ViewModels.Comment.ViewModels;
    using SdvCode.ViewModels.Post.ViewModels;
    using SdvCode.ViewModels.Tag;

    public class PostProfile : Profile
    {
        public PostProfile()
        {
            this.CreateMap<Comment, CommentViewModel>();
            this.CreateMap<PostImage, PostImageViewModel>();
            this.CreateMap<Post, PostViewModel>();
            //this.CreateMap<List<Post>, List<PostViewModel>>();
        }
    }
}