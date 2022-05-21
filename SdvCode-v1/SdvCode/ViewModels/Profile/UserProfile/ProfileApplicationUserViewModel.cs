// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Profile.UserProfile
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProfileApplicationUserViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string AboutMe { get; set; }

        public string ImageUrl { get; set; }

        public string CoverImageUrl { get; set; }

        public string ZipCodeId { get; set; }

        public ProfileZipCodeViewModel ZipCode { get; set; }

        public string CountryId { get; set; }

        public ProfileCountryViewModel Country { get; set; }

        public string StateId { get; set; }

        public ProfileStateViewModel State { get; set; }

        public string CityId { get; set; }

        public ProfileCityViewModel City { get; set; }

        public string CountryCodeId { get; set; }

        public ProfileCountryCodeViewModel CountryCode { get; set; }

        public string GitHubUrl { get; set; }

        public string StackoverflowUrl { get; set; }

        public string FacebookUrl { get; set; }

        public string LinkedinUrl { get; set; }

        public string TwitterUrl { get; set; }

        public string InstagramUrl { get; set; }

        public bool IsBlocked { get; set; }

        public string ReasonToBeBlocked { get; set; }

        public ICollection<ProfileRoleViewModel> Roles { get; set; } = new HashSet<ProfileRoleViewModel>();

        public string GroupName { get; set; }

        public bool IsFollowed { get; set; }

        public int ActionsCount { get; set; }

        public int CreatedPosts { get; set; }

        public int LikedPosts { get; set; }

        public int CommentsCount { get; set; }
    }
}