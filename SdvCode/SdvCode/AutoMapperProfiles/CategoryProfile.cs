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
    using SdvCode.Models.Enums;
    using SdvCode.ViewModels.AllCategories.ViewModels;
    using SdvCode.ViewModels.Blog.ViewModels;
    using SdvCode.ViewModels.Blog.ViewModels.BlogPostCard;
    using SdvCode.ViewModels.Category.ViewModels;
    using SdvCode.ViewModels.Category.ViewModels.TopCategory;
    using SdvCode.ViewModels.Post.ViewModels;
    using SdvCode.ViewModels.Post.ViewModels.PostPage;

    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            this.CreateMap<Category, BlogPostCardCategoryViewModel>();
            this.CreateMap<Category, PostCategoryViewModel>();

            this.CreateMap<Category, AllCategoriesCategoryViewModel>()
                .ForMember(
                    dm => dm.ApprovedPostsCount,
                    mo => mo.MapFrom(x => x.Posts.Count(x => x.PostStatus == PostStatus.Approved)))
                .ForMember(
                    dm => dm.BannedPostsCount,
                    mo => mo.MapFrom(x => x.Posts.Count(x => x.PostStatus == PostStatus.Banned)))
                .ForMember(
                    dm => dm.PendingPostsCount,
                    mo => mo.MapFrom(x => x.Posts.Count(x => x.PostStatus == PostStatus.Pending)));

            this.CreateMap<Category, TopCategoryViewModel>()
                .ForMember(
                    dm => dm.PostsCount,
                    mo => mo.MapFrom(x => x.Posts.Count(x => x.PostStatus == PostStatus.Approved)));
        }
    }
}