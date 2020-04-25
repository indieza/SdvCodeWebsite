// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.Shop.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using MoreLinq.Extensions;
    using SdvCode.Areas.Administration.ViewModels.ShopViewModels.ViewModels;
    using SdvCode.Data;

    public class OrdersService : IOrdersService
    {
        private readonly ApplicationDbContext db;

        public OrdersService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<ICollection<ShopOrderViewModel>> GetAllOrders()
        {
            var allOrders = this.db.Orders.ToList();
            var result = new List<ShopOrderViewModel>();

            foreach (var order in allOrders.ToList())
            {
                var currentOrderModel = new ShopOrderViewModel
                {
                    Email = order.Email,
                    Address = $"{order.Country}, {order.City}, {order.Address}",
                    PhoneNumber = order.PhoneNumber,
                    UserFullName = $"{order.FirstName} {order.LastName}",
                    AditionalInformation = order.AditionalInfromation,
                    ZipCode = order.ZipCode,
                };

                var orderProducts = this.db.OrderProducts.Where(x => x.OrderId == order.Id).ToList();
                foreach (var orderProduct in orderProducts)
                {
                    var product = await this.db.Products
                        .FirstOrDefaultAsync(x => x.Id == orderProduct.ProductId);
                    currentOrderModel.Products.Add(new OrderedProductViewModel
                    {
                        Name = product.Name,
                        AvailableQuantity = product.AvailableQuantity,
                        Price = product.Price,
                        WantedQuantity = Math.Min(product.AvailableQuantity, orderProduct.WantedQuantity),
                        TotalProductPrice =
                        product.Price * Math.Min(product.AvailableQuantity, orderProduct.WantedQuantity),
                    });
                }

                currentOrderModel.TotalPrice = currentOrderModel.Products.Sum(x => x.TotalProductPrice);
                result.Add(currentOrderModel);
            }

            return result;
        }
    }
}