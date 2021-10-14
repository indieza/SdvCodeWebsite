// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.AutoMapperProfiles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Http;

    using SdvCode.Data;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Blog.ViewModels.BlogPostCard;
    using SdvCode.ViewModels.Comment.ViewModels.RecentComment;
    using SdvCode.ViewModels.Home;
    using SdvCode.ViewModels.Post.ViewModels;
    using SdvCode.ViewModels.Post.ViewModels.PostPage;
    using SdvCode.ViewModels.Post.ViewModels.RecentPost;
    using SdvCode.ViewModels.Post.ViewModels.TopPost;
    using SdvCode.ViewModels.Users.ViewModels;

    public class UserProfile : Profile
    {
        private readonly ApplicationDbContext db;

        public UserProfile(ApplicationDbContext db)
        {
            this.db = db;

            this.CreateMap<ApplicationUser, BlogPostCardApplicationUserViewModel>();
            this.CreateMap<ApplicationUser, BlogPostCardLikerViewModel>();
            this.CreateMap<ApplicationUser, PostApplicationUserViewModel>();
            this.CreateMap<ApplicationUser, PostLikerViewModel>();
            this.CreateMap<ApplicationUser, TopPostApplicationUserViewMdoel>();
            this.CreateMap<ApplicationUser, RecentPostApplicationUserViewModel>();
            this.CreateMap<ApplicationUser, RecentCommentApplicationUserViewModel>();
            this.CreateMap<ApplicationUser, HomeAdministratorUserViewModel>();
        }
    }
}