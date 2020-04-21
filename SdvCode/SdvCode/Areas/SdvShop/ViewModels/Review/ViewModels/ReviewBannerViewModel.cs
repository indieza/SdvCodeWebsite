// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.ViewModels.Review.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class ReviewBannerViewModel
    {
        [Required]
        public decimal Rating { get; set; }

        [Required]
        public int ReviewsCount { get; set; }

        public IDictionary<int, int> Stars { get; set; } = new Dictionary<int, int>();
    }
}