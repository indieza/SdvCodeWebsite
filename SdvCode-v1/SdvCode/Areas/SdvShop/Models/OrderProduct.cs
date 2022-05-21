// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;

    public class OrderProduct
    {
        [Required]
        [ForeignKey(nameof(Product))]
        public string ProductId { get; set; }

        public Product Product { get; set; }

        [Required]
        [ForeignKey(nameof(Order))]
        public string OrderId { get; set; }

        public Order Order { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int WantedQuantity { get; set; }
    }
}