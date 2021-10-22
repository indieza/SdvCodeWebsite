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
    using SdvCode.Areas.UserNotifications.Models;
    using SdvCode.Constraints;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;

    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
        }

        [ForeignKey(nameof(ZipCode))]
        public string ZipCodeId { get; set; }

        public ZipCode ZipCode { get; set; }

        [ForeignKey(nameof(Country))]
        public string CountryId { get; set; }

        public Country Country { get; set; }

        [ForeignKey(nameof(State))]
        public string StateId { get; set; }

        public State State { get; set; }

        [ForeignKey(nameof(City))]
        public string CityId { get; set; }

        public City City { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime RegisteredOn { get; set; }

        public Gender Gender { get; set; }

        [ForeignKey(nameof(CountryCode))]
        public string CountryCodeId { get; set; }

        public CountryCode CountryCode { get; set; }

        [MaxLength(ModelConstraints.ApplicationUserAboutMeMaxLength)]
        public string AboutMe { get; set; }

        [MaxLength(ModelConstraints.ApplicationUserFirstNameMaxLength)]
        public string FirstName { get; set; }

        [MaxLength(ModelConstraints.ApplicationUserLastNameMaxLength)]
        public string LastName { get; set; }

        public string ImageUrl { get; set; }

        public string CoverImageUrl { get; set; }

        public string GitHubUrl { get; set; }

        public string StackoverflowUrl { get; set; }

        public string FacebookUrl { get; set; }

        public string LinkedinUrl { get; set; }

        public string TwitterUrl { get; set; }

        public string InstagramUrl { get; set; }

        [Required]
        public bool IsBlocked { get; set; }

        [MaxLength(ModelConstraints.ApplicationUserReasonToBeBlockedMaxLength)]
        public string ReasonToBeBlocked { get; set; }

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

        public virtual ICollection<UserAction> UserActions { get; set; }

        public virtual ICollection<FollowUnfollow> Following { get; set; }

        public virtual ICollection<FollowUnfollow> Followers { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<PostLike> PostLikes { get; set; }

        public virtual ICollection<FavouritePost> FavouritePosts { get; set; }

        public virtual ICollection<PendingPost> PendingPosts { get; set; }

        public virtual ICollection<BlockedPost> BlockedPosts { get; set; }

        public virtual ICollection<UserGroup> UsersGroups { get; set; }

        public virtual ICollection<ChatMessage> ChatMessages { get; set; }

        public virtual ICollection<ChatImage> ChatImages { get; set; }

        public virtual ICollection<RecommendedFriend> RecommendedFriends { get; set; }

        public virtual ICollection<ProductComment> ProductComments { get; set; }

        public virtual ICollection<ProductReview> ProductReviews { get; set; }

        public virtual ICollection<UserNotification> UserNotifications { get; set; }

        public virtual ICollection<QuickChatReply> QuickChatReplies { get; set; }

        public virtual ICollection<FavouriteStickers> FavouriteStickers { get; set; }
    }
}