// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.AutoMapperProfiles.User
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
    using SdvCode.ViewModels.Profile.UserProfile;
    using SdvCode.ViewModels.Profile.UserViewComponents;
    using SdvCode.ViewModels.Profile.UserViewComponents.ActivitiesComponent;
    using SdvCode.ViewModels.Profile.UserViewComponents.BlogComponent;
    using SdvCode.ViewModels.Users.ViewModels;

    public class UserProfile : Profile
    {
        private readonly ApplicationDbContext db;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserProfile(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            this.db = db;
            this.httpContextAccessor = httpContextAccessor;

            var userId = this.httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            this.CreateMap<ApplicationUser, BlogPostCardApplicationUserViewModel>();
            this.CreateMap<ApplicationUser, BlogPostCardLikerViewModel>();
            this.CreateMap<ApplicationUser, PostApplicationUserViewModel>();
            this.CreateMap<ApplicationUser, PostLikerViewModel>();
            this.CreateMap<ApplicationUser, TopPostApplicationUserViewMdoel>();
            this.CreateMap<ApplicationUser, RecentPostApplicationUserViewModel>();
            this.CreateMap<ApplicationUser, RecentCommentApplicationUserViewModel>();
            this.CreateMap<ApplicationUser, HomeAdministratorUserViewModel>();

            this.CreateMap<ApplicationUser, ProfileApplicationUserViewModel>()
                .ForMember(
                    dm => dm.IsFollowed,
                    mo => mo.MapFrom(x => this.db.FollowUnfollows
                        .Any(y => y.FollowerId == userId && y.PersonId == x.Id && y.IsFollowed == true)))
                .ForMember(
                    dm => dm.ActionsCount,
                    mo => mo.MapFrom(x => x.UserActions.Count))
                .ForMember(
                    dm => dm.CreatedPosts,
                    mo => mo.MapFrom(x => x.Posts.Count))
                .ForMember(
                    dm => dm.LikedPosts,
                    mo => mo.MapFrom(x => x.PostLikes.Count))
                .ForMember(
                    dm => dm.CommentsCount,
                    mo => mo.MapFrom(x => x.Comments.Count))
                .ForMember(
                    dm => dm.Roles,
                    mo => mo.MapFrom(x => x.UserRoles.Select(y => y.Role)));

            this.CreateMap<ApplicationUser, ActivitiesApplicationUserViewModel>();
            this.CreateMap<ApplicationUser, BlogComponentApplicationUserViewModel>();
        }
    }
}