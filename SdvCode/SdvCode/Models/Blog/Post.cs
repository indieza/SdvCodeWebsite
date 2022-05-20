﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Models.Blog
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Formatters;

    using SdvCode.Constraints;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;

    public class Post
    {
        public Post()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Likes = 0;
            this.CreatedOn = DateTime.UtcNow;
        }

        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        [MaxLength(ModelConstraints.BlogPostTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [MaxLength(ModelConstraints.BlogPostShortContentMaxLength)]
        public string ShortContent { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int Likes { get; set; }

        [Required]
        [EnumDataType(typeof(PostStatus))]
        public PostStatus PostStatus { get; set; }

        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        [ForeignKey(nameof(Category))]
        public string CategoryId { get; set; }

        public Category Category { get; set; }

        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public ICollection<PostTag> PostsTags { get; set; } = new HashSet<PostTag>();

        public ICollection<FavouritePost> FavouritePosts { get; set; } = new HashSet<FavouritePost>();

        public ICollection<PendingPost> PendingPosts { get; set; } = new HashSet<PendingPost>();

        public ICollection<BlockedPost> BlockedPosts { get; set; } = new HashSet<BlockedPost>();

        public ICollection<UserAction> UserActions { get; set; } = new HashSet<UserAction>();

        public ICollection<PostImage> PostImages { get; set; } = new HashSet<PostImage>();

        public ICollection<PostLike> PostLikes { get; set; }
    }
}