// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Post.ViewModels.PostPage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SdvCode.Models.Enums;

    public class PostViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string ImageUrl { get; set; }

        public int Likes { get; set; }

        public PostStatus PostStatus { get; set; }

        public PostApplicationUserViewModel ApplicationUser { get; set; }

        public PostCategoryViewModel Category { get; set; }

        public ICollection<PostCommentViewModel> Comments { get; set; } = new HashSet<PostCommentViewModel>();

        public ICollection<PostTagViewModel> Tags { get; set; } = new HashSet<PostTagViewModel>();

        public ICollection<PostPostImageViewModel> PostImages { get; set; } = new HashSet<PostPostImageViewModel>();

        public ICollection<PostLikerViewModel> Likers { get; set; } = new HashSet<PostLikerViewModel>();

        public bool IsLiked { get; set; }

        public bool IsFavourite { get; set; }

        public bool IsAuthor { get; set; }
    }
}