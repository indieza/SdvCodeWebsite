// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.BlogAddonsViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class AddCategoryInputModel
    {
        [Required]
        [MaxLength(30)]
        [Display(Name = "Category name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(300)]
        [Display(Name = "Category description")]
        public string Description { get; set; }
    }
}