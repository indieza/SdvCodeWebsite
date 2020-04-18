// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.ViewModels.Product.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Ganss.XSS;
    using Microsoft.AspNetCore.Http;

    public class EditProductInputModel
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
        public string SanitaizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        [Required]
        public string SanitaizedSpecifications => new HtmlSanitizer().Sanitize(this.SpecificationsDescription);

        [Required]
        [Display(Name = "Product Price")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Product Category")]
        public string ProductCategory { get; set; }

        public List<IFormFile> ProductImages { get; set; } = new List<IFormFile>();
    }
}