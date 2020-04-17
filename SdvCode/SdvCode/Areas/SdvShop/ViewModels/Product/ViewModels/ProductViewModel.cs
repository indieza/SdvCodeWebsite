using SdvCode.Areas.SdvShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Areas.SdvShop.ViewModels.Product.ViewModels
{
    public class ProductViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string ProductCategoryId { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; } = new HashSet<ProductImage>();
    }
}