// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.DataViewModels.Blog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Category;
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

        public CategoryViewModel Category { get; set; }

        public ICollection<CommentViewModel> Comments { get; set; } = new HashSet<CommentViewModel>();

        public ICollection<TagViewModel> Tags { get; set; } = new HashSet<TagViewModel>();

        public ICollection<FavouritePostViewModel> FavouritePosts { get; set; } = new HashSet<FavouritePostViewModel>();

        public ICollection<PendingPostViewModel> PendingPosts { get; set; } = new HashSet<PendingPostViewModel>();

        public ICollection<BlockedPostViewModel> BlockedPosts { get; set; } = new HashSet<BlockedPostViewModel>();

        public ICollection<UserAction> UserActions { get; set; } = new HashSet<UserAction>();

        public ICollection<PostImageViewModel> PostImages { get; set; } = new HashSet<PostImageViewModel>();

        public ICollection<PostLikeViewModel> PostLikes { get; set; }

        public ICollection<ApplicationUserViewModel> Likers { get; set; } = new HashSet<ApplicationUserViewModel>();

        public bool IsAuthor { get; set; }

        public bool IsLiked { get; set; }

        public bool IsFavourite { get; set; }
    }
}