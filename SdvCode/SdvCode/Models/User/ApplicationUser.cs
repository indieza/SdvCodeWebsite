// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Models.User
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.AspNetCore.Identity;
    using SdvCode.Areas.PrivateChat.Models;
    using SdvCode.Areas.SdvShop.Models;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;

    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
        }

        [ForeignKey(nameof(ZipCode))]
        public int? ZipCodeId { get; set; }

        public ZipCode ZipCode { get; set; }

        [ForeignKey(nameof(Country))]
        public int? CountryId { get; set; }

        public Country Country { get; set; }

        [ForeignKey(nameof(State))]
        public int? StateId { get; set; }

        public State State { get; set; }

        [ForeignKey(nameof(City))]
        public int? CityId { get; set; }

        public City City { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime RegisteredOn { get; set; }

        public Gender Gender { get; set; }

        public CountryCode CountryCode { get; set; }

        [MaxLength(600)]
        public string AboutMe { get; set; }

        [MaxLength(15)]
        public string FirstName { get; set; }

        [MaxLength(15)]
        public string LastName { get; set; }

        public string ImageUrl { get; set; }

        public string CoverImageUrl { get; set; }

        public string GitHubUrl { get; set; }

        public string StackoverflowUrl { get; set; }

        public string FacebookUrl { get; set; }

        public string LinkedinUrl { get; set; }

        public string TwitterUrl { get; set; }

        public string InstagramUrl { get; set; }

        public bool IsBlocked { get; set; }

        // public ICollection<FavouriteProduct> FavouriteProducts { get; set; } = new HashSet<FavouriteProduct>();

        // public ICollection<WishlistProduct> WishlistProducts { get; set; } = new HashSet<WishlistProduct>();
        public ICollection<UserAction> UserActions { get; set; } = new HashSet<UserAction>();

        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();

        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public ICollection<FavouritePost> FavouritePosts { get; set; } = new HashSet<FavouritePost>();

        public ICollection<PendingPost> PendingPosts { get; set; } = new HashSet<PendingPost>();

        public ICollection<BlockedPost> BlockedPosts { get; set; } = new HashSet<BlockedPost>();

        public ICollection<UserGroup> UsersGroups { get; set; } = new HashSet<UserGroup>();

        public ICollection<ChatMessage> ChatMessages { get; set; } = new HashSet<ChatMessage>();

        public ICollection<RecommendedFriend> RecommendedFriends { get; set; } = new HashSet<RecommendedFriend>();

        public ICollection<ProductComment> ProductComments { get; set; } = new HashSet<ProductComment>();

        public ICollection<ProductReview> ProductReviews { get; set; } = new HashSet<ProductReview>();
    }
}