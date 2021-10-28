// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Models.User.UserActions.BlogActions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;

    using SdvCode.Constraints;

    public class DeletedPostUserAction
    {
        public DeletedPostUserAction()
        {
        }

        [Required]
        [MaxLength(ModelConstraints.BlogPostTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(ModelConstraints.BlogPostShortContentMaxLength)]
        public string ShortContent { get; set; }

        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string DeleterApplicationUserId { get; set; }

        public ApplicationUser DeleterApplicationUser { get; set; }
    }
}