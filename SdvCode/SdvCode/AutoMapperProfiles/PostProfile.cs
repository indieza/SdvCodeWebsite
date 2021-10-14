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

    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.AllCategories.ViewModels;
    using SdvCode.ViewModels.Blog.ViewModels.BlogPostCard;
    using SdvCode.ViewModels.Category;
    using SdvCode.ViewModels.Comment.ViewModels;
    using SdvCode.ViewModels.Home;
    using SdvCode.ViewModels.Post.InputModels;
    using SdvCode.ViewModels.Post.ViewModels;
    using SdvCode.ViewModels.Post.ViewModels.PostPage;
    using SdvCode.ViewModels.Post.ViewModels.RecentPost;
    using SdvCode.ViewModels.Post.ViewModels.TopPost;
    using SdvCode.ViewModels.Tag;

    public class PostProfile : Profile
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public PostProfile(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            var userId = this.httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            this.CreateMap<Post, BlogPostCardViewModel>()
                .ForMember(
                    dm => dm.CommentsCount,
                    mo => mo.MapFrom(x => x.Comments.Count))
                .ForMember(
                    dm => dm.IsLiked,
                    mo => mo.MapFrom(x => userId != null && x.PostLikes.Any(y => y.UserId == userId && y.IsLiked)))
                .ForMember(
                    dm => dm.IsAuthor,
                    mo => mo.MapFrom(x => userId != null && userId == x.ApplicationUserId))
                .ForMember(
                    dm => dm.IsFavourite,
                    mo => mo.MapFrom(x => userId != null && x.FavouritePosts.Any(z => z.ApplicationUserId == userId && z.IsFavourite)))
                .ForMember(
                    dm => dm.Likers,
                    mo => mo.MapFrom(x => x.PostLikes.Where(x => x.IsLiked).Select(x => x.ApplicationUser).ToList()));

            this.CreateMap<Post, PostViewModel>()
                .ForMember(
                    dm => dm.IsLiked,
                    mo => mo.MapFrom(x => userId != null && x.PostLikes.Any(y => y.UserId == userId && y.IsLiked)))
                .ForMember(
                    dm => dm.IsAuthor,
                    mo => mo.MapFrom(x => userId != null && userId == x.ApplicationUserId))
                .ForMember(
                    dm => dm.IsFavourite,
                    mo => mo.MapFrom(x => userId != null && x.FavouritePosts.Any(z => z.ApplicationUserId == userId && z.IsFavourite)))
                .ForMember(
                    dm => dm.Likers,
                    mo => mo.MapFrom(x => x.PostLikes.Where(x => x.IsLiked).Select(x => x.ApplicationUser)))
                .ForMember(
                    dm => dm.Tags,
                    mo => mo.MapFrom(x => x.PostsTags.Select(x => x.Tag).ToList()));

            this.CreateMap<Post, AllCategoriesPostViewModel>();
            this.CreateMap<Post, TopPostViewModel>();
            this.CreateMap<Post, RecentPostViewModel>();

            this.CreateMap<Post, EditPostInputModel>()
                .ForMember(
                    dm => dm.CategoryName,
                    mo => mo.MapFrom(x => x.Category.Name));

            this.CreateMap<Post, HomeLatestPostViewModel>()
                .ForMember(
                    dm => dm.AuthorUsername,
                    mo => mo.MapFrom(x => x.ApplicationUser.UserName))
                .ForMember(
                    dm => dm.CategoryName,
                    mo => mo.MapFrom(x => x.Category.Name));
        }
    }
}