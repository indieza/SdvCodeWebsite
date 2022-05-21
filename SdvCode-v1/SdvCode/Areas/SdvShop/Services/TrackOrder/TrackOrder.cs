// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.Services.TrackOrder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.SdvShop.ViewModels.Order;
    using SdvCode.Areas.SdvShop.ViewModels.TrackOrder;
    using SdvCode.Areas.SdvShop.ViewModels.TrackOrder.InputModels;
    using SdvCode.Areas.SdvShop.ViewModels.TrackOrder.ViewModels;
    using SdvCode.Data;

    public class TrackOrder : ITrackOrder
    {
        private readonly ApplicationDbContext db;

        public TrackOrder(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<TrackOrderViewModel> GetOrder(TrackOrderInputModel model)
        {
            var order = await this.db.Orders
                .FirstOrDefaultAsync(x => x.Id == model.OrderId && x.Email == model.Email);

            if (order != null)
            {
                var orderProducts = this.db.OrderProducts.Where(x => x.OrderId == order.Id).ToList();
                var result = new TrackOrderViewModel
                {
                    Id = order.Id,
                    OrderStatus = order.OrderStatus,
                    CreatedOn = order.CreatedOn,
                    FinishedOn = order.FinishedOn,
                    CanceledOn = order.CanceledOn,
                };

                foreach (var orderProduct in orderProducts)
                {
                    var targetProduct = await this.db.Products
                        .FirstOrDefaultAsync(x => x.Id == orderProduct.ProductId);
                    var image = await this.db.ProductImages
                        .Where(x => x.ProductId == orderProduct.ProductId)
                        .OrderBy(x => x.Name)
                        .FirstOrDefaultAsync();
                    var product = new ProductInCartViewModel
                    {
                        Id = targetProduct.Id,
                        Name = targetProduct.Name,
                        AvailableQuantity = targetProduct.AvailableQuantity,
                        Price = targetProduct.Price,
                        WantedQuantity = orderProduct.WantedQuantity,
                        TotalProductPrice = targetProduct.Price * orderProduct.WantedQuantity,
                        ImageUrl = image.ImageUrl,
                    };
                    result.Products.Add(product);
                }

                return result;
            }

            return null;
        }
    }
}