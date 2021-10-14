// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.AutoMapperProfiles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using SdvCode.Models.Blog;
    using SdvCode.ViewModels.Comment.ViewModels.RecentComment;
    using SdvCode.ViewModels.Post.ViewModels;
    using SdvCode.ViewModels.Post.ViewModels.PostPage;

    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            this.CreateMap<Comment, PostCommentViewModel>();
            this.CreateMap<Comment, RecentCommentViewModel>();
        }
    }
}