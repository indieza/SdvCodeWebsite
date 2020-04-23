// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.SdvShop.ViewModels.Order;
    using SdvCode.Data;

    public class ShoppingCartState
    {
        private readonly ApplicationDbContext db;

        public ShoppingCartState(ApplicationDbContext db)
        {
            this.db = db;
        }

        public event Action OnChange;

        public Dictionary<string, int> ShoppingCart { get; private set; } = new Dictionary<string, int>();

        public void AddProduct(string productId, int quantity)
        {
            if (!this.ShoppingCart.ContainsKey(productId))
            {
                this.ShoppingCart.Add(productId, 0);
            }

            this.ShoppingCart[productId] += quantity;
            this.NotifyStateChanged();
        }

        public async Task<ICollection<ProductInCartViewModel>> ExtractAllProducts()
        {
            var result = new List<ProductInCartViewModel>();

            foreach (var targetItem in this.ShoppingCart)
            {
                var product = await this.db.Products.FirstOrDefaultAsync(x => x.Id == targetItem.Key);
                var image = await this.db.ProductImages
                    .Where(x => x.ProductId == product.Id)
                    .OrderBy(x => x.Name)
                    .Select(x => x.ImageUrl)
                    .FirstOrDefaultAsync();
                result.Add(new ProductInCartViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    WantedQuantity = targetItem.Value,
                    Price = product.Price,
                    ImageUrl = image,
                    TotalProductPrice = targetItem.Value * product.Price,
                });
            }

            return result;
        }

        private void NotifyStateChanged() => this.OnChange?.Invoke();
    }
}