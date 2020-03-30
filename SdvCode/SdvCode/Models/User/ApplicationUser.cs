// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Models.User
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.AspNetCore.Identity;
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

        //[ForeignKey(nameof(Address))]
        //public string AddressId { get; set; }

        //public Address Address { get; set; }

        //[ForeignKey(nameof(ShoppingCart))]
        //public string ShoppingCartId { get; set; }

        //public ShoppingCart ShoppingCart { get; set; }

        //public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();

        //public ICollection<FavouriteProduct> FavouriteProducts { get; set; } = new HashSet<FavouriteProduct>();

        //public ICollection<WishlistProduct> WishlistProducts { get; set; } = new HashSet<WishlistProduct>();

        //public ICollection<Order> Orders { get; set; } = new HashSet<Order>();

        public ICollection<UserAction> UserActions { get; set; } = new HashSet<UserAction>();

        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();

        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public ICollection<FavouritePost> FavouritePosts { get; set; } = new HashSet<FavouritePost>();

        public ICollection<PendingPost> PendingPosts { get; set; } = new HashSet<PendingPost>();

        public ICollection<BlockedPost> BlockedPosts { get; set; } = new HashSet<BlockedPost>();

        //[NotMapped]
        //public int ActionsCount { get; set; }

        //[NotMapped]
        //public ICollection<ApplicationRole> Roles { get; set; } = new HashSet<ApplicationRole>();

        //[NotMapped]
        //public bool IsFollowed { get; set; }
    }
}