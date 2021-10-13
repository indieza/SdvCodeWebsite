// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Blog.ViewModels.BlogPostCard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SdvCode.Models.Enums;

    public class BlogPostCardViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string ShortContent { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string ImageUrl { get; set; }

        public int Likes { get; set; }

        public PostStatus PostStatus { get; set; }

        public BlogPostCardApplicationUserViewModel ApplicationUser { get; set; }

        public BlogPostCardCategoryViewModel Category { get; set; }

        public int CommentsCount { get; set; }

        public bool IsAuthor { get; set; }

        public bool IsLiked { get; set; }

        public bool IsFavourite { get; set; }

        public ICollection<BlogPostCardLikerViewModel> Likers { get; set; } = new HashSet<BlogPostCardLikerViewModel>();
    }
}