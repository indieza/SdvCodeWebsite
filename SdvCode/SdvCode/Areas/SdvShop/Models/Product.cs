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
    using Microsoft.AspNetCore.Http;

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
        [Column(TypeName = "decimal(15, 2)")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int AvailableQuantity { get; set; }

        [Required]
        public string SpecificationsDescription { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        [Required]
        [ForeignKey(nameof(ProductCategory))]
        public string ProductCategoryId { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; } = new HashSet<ProductImage>();

        //public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();

        //public ICollection<FavouriteProduct> FavouriteProducts { get; set; } = new HashSet<FavouriteProduct>();

        //public ICollection<ShoppingCartProduct> ShoppingCartProducts { get; set; } = new HashSet<ShoppingCartProduct>();
    }
}