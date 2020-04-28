// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.ViewModels.TrackOrder.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Areas.SdvShop.Models.Enums;
    using SdvCode.Areas.SdvShop.ViewModels.Order;

    public class TrackOrderViewModel
    {
        public string Id { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public DateTime? FinishedOn { get; set; }

        public DateTime? CanceledOn { get; set; }

        [Required]
        public OrderStatus OrderStatus { get; set; }

        public ICollection<ProductInCartViewModel> Products { get; set; } = new HashSet<ProductInCartViewModel>();
    }
}