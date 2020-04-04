// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Comment
{
    using System.ComponentModel.DataAnnotations;
    using Ganss.XSS;

    public class CreateCommentInputModel
    {
        public string PostId { get; set; }

        public string ParentId { get; set; }

        [Required]
        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);
    }
}