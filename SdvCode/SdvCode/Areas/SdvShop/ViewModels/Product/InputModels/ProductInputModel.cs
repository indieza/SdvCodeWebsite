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
    using SdvCode.Areas.SdvShop.Models;

    public class ProductInputModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string SanitaizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string ProductCategory { get; set; }

        public List<IFormFile> ProductImages { get; set; } = new List<IFormFile>();
    }
}