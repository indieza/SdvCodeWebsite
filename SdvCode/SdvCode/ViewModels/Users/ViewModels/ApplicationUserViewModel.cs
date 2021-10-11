﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Users.ViewModels
{
    using System;
    using System.Collections.Generic;

    using SdvCode.Areas.PrivateChat.Models;
    using SdvCode.Areas.SdvShop.Models;
    using SdvCode.Areas.UserNotifications.Models;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Comment.ViewModels;
    using SdvCode.ViewModels.Post.ViewModels;

    public class ApplicationUserViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public string ZipCodeId { get; set; }

        public ZipCode ZipCode { get; set; }

        public string CountryId { get; set; }

        public Country Country { get; set; }

        public string StateId { get; set; }

        public State State { get; set; }

        public string CityId { get; set; }

        public City City { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime RegisteredOn { get; set; }

        public Gender Gender { get; set; }

        public string CountryCodeId { get; set; }

        public CountryCode CountryCode { get; set; }

        public string AboutMe { get; set; }

        public string FirstName { get; set; }

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

        public string ReasonToBeBlocked { get; set; }

        public ICollection<UserAction> UserActions { get; set; } = new HashSet<UserAction>();

        public ICollection<PostViewModel> Posts { get; set; } = new HashSet<PostViewModel>();

        public ICollection<CommentViewModel> Comments { get; set; } = new HashSet<CommentViewModel>();

        public ICollection<PostLike> PostLikes { get; set; } = new HashSet<PostLike>();

        public ICollection<FavouritePost> FavouritePosts { get; set; } = new HashSet<FavouritePost>();

        public ICollection<PendingPost> PendingPosts { get; set; } = new HashSet<PendingPost>();

        public ICollection<BlockedPost> BlockedPosts { get; set; } = new HashSet<BlockedPost>();

        public ICollection<UserGroup> UsersGroups { get; set; } = new HashSet<UserGroup>();

        public ICollection<ChatMessage> ChatMessages { get; set; } = new HashSet<ChatMessage>();

        public ICollection<ChatImage> ChatImages { get; set; } = new HashSet<ChatImage>();

        public ICollection<RecommendedFriend> RecommendedFriends { get; set; } = new HashSet<RecommendedFriend>();

        public ICollection<ProductComment> ProductComments { get; set; } = new HashSet<ProductComment>();

        public ICollection<ProductReview> ProductReviews { get; set; } = new HashSet<ProductReview>();

        public ICollection<UserNotification> UserNotifications { get; set; } = new HashSet<UserNotification>();

        public ICollection<QuickChatReply> QuickChatReplies { get; set; } = new HashSet<QuickChatReply>();

        public ICollection<FavouriteStickers> FavouriteStickers { get; set; } = new HashSet<FavouriteStickers>();

        public int ActionsCount { get; set; }

        public ICollection<ApplicationRole> Roles { get; set; } = new HashSet<ApplicationRole>();

        public bool IsFollowed { get; set; }

        public string GroupName { get; set; }
    }
}