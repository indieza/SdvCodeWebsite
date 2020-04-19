// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.ViewModels.Comment.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Ganss.XSS;

    public class AddCommentInputModel
    {
        [Required(ErrorMessage = "Full name is required - range [0-50]")]
        [MaxLength(50)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Phone number is required - range [0-20]")]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        public string Username { get; set; }

        [Required(ErrorMessage = "Product Id is required")]
        public string ProductId { get; set; }

        public string ParentId { get; set; }

        [Required(ErrorMessage = "Comment content is required")]
        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);
    }
}