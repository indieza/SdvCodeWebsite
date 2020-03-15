﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Models.User
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.AspNetCore.Identity;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;

    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
        }

        [MaxLength(20)]
        public string Country { get; set; }

        [MaxLength(20)]
        public string City { get; set; }

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

        public ICollection<UserAction> UserActions { get; set; } = new HashSet<UserAction>();

        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();

        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        [NotMapped]
        public int ActionsCount { get; set; }

        [NotMapped]
        public ICollection<IdentityRole> Roles { get; set; } = new HashSet<IdentityRole>();

        [NotMapped]
        public bool IsFollowed { get; set; }
    }
}