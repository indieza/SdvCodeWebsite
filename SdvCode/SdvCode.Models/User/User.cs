// <copyright file="User.cs" company="SDV Code Data Models">
// Copyright (c) SDV Code Data Models. All rights reserved.
// </copyright>

namespace SdvCode.Models.User
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.AspNetCore.Identity;

    using SdvCode.Constraints;
    using SdvCode.Models.UserInformation;
    using SdvCode.Models.WebsiteActions;
    using SdvCode.Models.WebsiteActions.Post;

    public class User : IdentityUser
    {
        public User()
        {
            this.IsBlocked = false;
        }

        [MaxLength(DataModelConstants.UserNamesMaxLength)]
        public string? FirstName { get; set; }

        [MaxLength(DataModelConstants.UserNamesMaxLength)]
        public string? MiddleName { get; set; }

        [MaxLength(DataModelConstants.UserNamesMaxLength)]
        public string? LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        [MaxLength(DataModelConstants.UserAboutMeMaxLength)]
        public string? AboutMe { get; set; }

        public string? ImageUrl { get; set; }

        public string? CoverImageUrl { get; set; }

        [MaxLength(DataModelConstants.UserSocialMediaLinkMaxLength)]
        public string? GitHubUrl { get; set; }

        [MaxLength(DataModelConstants.UserSocialMediaLinkMaxLength)]
        public string? StackoverflowUrl { get; set; }

        [MaxLength(DataModelConstants.UserSocialMediaLinkMaxLength)]
        public string? FacebookUrl { get; set; }

        [MaxLength(DataModelConstants.UserSocialMediaLinkMaxLength)]
        public string? LinkedinUrl { get; set; }

        [MaxLength(DataModelConstants.UserSocialMediaLinkMaxLength)]
        public string? TwitterUrl { get; set; }

        [MaxLength(DataModelConstants.UserSocialMediaLinkMaxLength)]
        public string? InstagramUrl { get; set; }

        [Required]
        public bool IsBlocked { get; set; }

        [MaxLength(DataModelConstants.UserReasonToBeBlockedMaxLength)]
        public string? ReasonToBeBlocked { get; set; }

        [ForeignKey(nameof(ZipCode))]
        public string? ZipCodeId { get; set; }

        public virtual ZipCode ZipCode { get; set; }

        [ForeignKey(nameof(Country))]
        public string? CountryId { get; set; }

        public virtual Country Country { get; set; }

        [ForeignKey(nameof(State))]
        public string? StateId { get; set; }

        public virtual State State { get; set; }

        [ForeignKey(nameof(City))]
        public string? CityId { get; set; }

        public virtual City City { get; set; }

        [ForeignKey(nameof(CountryCode))]
        public string? CountryCodeId { get; set; }

        public virtual CountryCode CountryCode { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();

        public virtual ICollection<BaseWebsiteAction> Actions { get; set; } = new HashSet<BaseWebsiteAction>();

        public virtual ICollection<LikedPostAction> LikedPostActions { get; set; } = new HashSet<LikedPostAction>();

        public virtual ICollection<UnlikedPostAction> UnlikedPostActions { get; set; } = new HashSet<UnlikedPostAction>();
    }
}