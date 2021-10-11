// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Post.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Comment.ViewModels;
    using SdvCode.ViewModels.Tag;
    using SdvCode.ViewModels.Users.ViewModels;

    public class PostViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ShortContent { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string ImageUrl { get; set; }

        public int Likes { get; set; }

        public PostStatus PostStatus { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUserViewModel ApplicationUser { get; set; }

        public string CategoryId { get; set; }

        public Category Category { get; set; }

        public ICollection<CommentViewModel> Comments { get; set; } = new HashSet<CommentViewModel>();

        public ICollection<TagViewModel> Tags { get; set; } = new HashSet<TagViewModel>();

        public bool IsAuthor { get; set; }

        public bool IsLiked { get; set; }

        public ICollection<ApplicationUserViewModel> Likers { get; set; } = new HashSet<ApplicationUserViewModel>();

        public ICollection<PostImageViewModel> AllPostImages { get; set; } = new HashSet<PostImageViewModel>();

        public bool IsFavourite { get; set; }
    }
}