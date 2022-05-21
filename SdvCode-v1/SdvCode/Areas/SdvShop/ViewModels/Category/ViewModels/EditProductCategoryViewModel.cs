// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.ViewModels.Category.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class EditProductCategoryViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Category Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Category Description")]
        public string Description { get; set; }
    }
}