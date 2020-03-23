// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.Models
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;

    public class Product
    {
        public Product()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        [Required]
        [ForeignKey(nameof(ProductCategory))]
        public string ProductCategoryId { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public ICollection<IFormFile> Images { get; set; } = new HashSet<IFormFile>();

        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();

        public ICollection<FavouriteProduct> FavouriteProducts { get; set; } = new HashSet<FavouriteProduct>();

        public ICollection<ShoppingCartProduct> ShoppingCartProducts { get; set; } = new HashSet<ShoppingCartProduct>();
    }
}