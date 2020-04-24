// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.Services.States.FavoriteProducts
{
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class FavoriteProductsState
    {
        private readonly ApplicationDbContext db;

        public FavoriteProductsState(ApplicationDbContext db)
        {
            this.db = db;
        }

        public event Action OnChange;

        public List<string> FavoriteProducts { get; private set; } = new List<string>();

        public void AddProduct(string productId)
        {
            if (!this.FavoriteProducts.Any(x => x == productId))
            {
                this.FavoriteProducts.Add(productId);
                this.NotifyStateChanged();
            }
        }

        public async Task<List<string>> UpdateSession(List<string> items)
        {
            var result = new List<string>();

            foreach (var id in items)
            {
                var targetProduct = await this.db.Products.FirstOrDefaultAsync(x => x.Id == id);

                if (targetProduct != null && targetProduct.AvailableQuantity > 0)
                {
                    result.Add(id);
                }
            }

            this.FavoriteProducts = result;
            this.NotifyStateChanged();
            return result;
        }

        private void NotifyStateChanged() => this.OnChange?.Invoke();
    }
}