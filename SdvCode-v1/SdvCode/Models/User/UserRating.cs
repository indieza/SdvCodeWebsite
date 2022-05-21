﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Models.User
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using SdvCode.Constraints;

    public class UserRating
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string RaterUsername { get; set; }

        [Required]
        [MaxLength(ModelConstraints.RatingStarsMaxValue)]
        public int Stars { get; set; }
    }
}