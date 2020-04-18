// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.ViewModels.Product.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class EditProductViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Product Description")]
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Product Availability")]
        public int AvailableQuantity { get; set; }

        [Required]
        [Display(Name = "Product Specifications")]
        public string SpecificationsDescription { get; set; }

        [Required]
        [Display(Name = "Product Price")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Product Category")]
        public string ProductCategory { get; set; }
    }
}