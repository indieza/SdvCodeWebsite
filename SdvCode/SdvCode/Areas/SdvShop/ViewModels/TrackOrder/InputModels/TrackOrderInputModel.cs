// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.ViewModels.TrackOrder.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class TrackOrderInputModel
    {
        [Required(ErrorMessage = "Order ID is required")]
        public string OrderId { get; set; }

        [Required(ErrorMessage = "Order email is required")]
        public string Email { get; set; }
    }
}