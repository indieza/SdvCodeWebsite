// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.Models.Enums
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public enum OrderStatus
    {
        [Display(Name = "Pending(Seller is considering your order.)")]
        Pending = 1,
        [Display(Name = "Approved(Seller approved the order and package it.)")]
        Approved = 2,
        [Display(Name = "Dispatched(Seller gave the order to th courier.)")]
        Dispatched = 3,
        [Display(Name = "Shipped(Your order is in the courier office.)")]
        Shipped = 4,
        [Display(Name = "Completed(Your order is finished. You take you products.)")]
        Completed = 5,
        [Display(Name = "Canceled(Your order is canceled. Contact with the seller for more information.)")]
        Canceled = 6,
        [Display(Name = "Declined(You close your own order for some reasons.)")]
        Declined = 7,
        [Display(Name = "Verification Required(Your order is on-hold, the seller will contact with you to approve it.)")]
        VerificaitonRequired = 8,
    }
}