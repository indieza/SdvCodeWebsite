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
        [Required]
        [MaxLength(50)]
        public string UserFullName { get; set; }

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Username { get; set; }

        [Required]
        public string ProductId { get; set; }

        public string ParentId { get; set; }

        [Required]
        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);
    }
}