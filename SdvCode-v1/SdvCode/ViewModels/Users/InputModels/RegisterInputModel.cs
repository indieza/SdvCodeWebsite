﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Users.InputModels
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterInputModel
    {
        [Required]
        [MinLength(length: 3)]
        [MaxLength(20)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}