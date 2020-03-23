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

    public class ShoppingCartProduct
    {
        [Required]
        [ForeignKey(nameof(ShoppingCart))]
        public string ShoppingCartId { get; set; }

        public ShoppingCart ShoppingCart { get; set; }

        [Required]
        [ForeignKey(nameof(Product))]
        public string ProductId { get; set; }

        public Product Product { get; set; }
    }
}