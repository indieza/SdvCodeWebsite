// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.Services.Order
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.SdvShop.Models;
    using SdvCode.Areas.SdvShop.ViewModels.Order;
    using SdvCode.Data;

    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext db;

        public OrderService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<OrderInformationViewModel> GetUserInformation(string username)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(x => x.UserName == username);
            if (user != null)
            {
                var country = await this.db.Countries.FirstOrDefaultAsync(x => x.Id == user.CountryId);
                var city = await this.db.Cities.FirstOrDefaultAsync(x => x.Id == user.CityId);
                var zipCode = await this.db.ZipCodes.FirstOrDefaultAsync(x => x.Id == user.ZipCodeId);

                return new OrderInformationViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    City = city?.Name,
                    Country = country?.Name,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    ZipCode = zipCode?.Code,
                    Address = null,
                    AditionalInfromation = null,
                };
            }

            return new OrderInformationViewModel();
        }

        public async Task PlaceOrder(ICollection<ProductInCartViewModel> products, OrderInformationViewModel userInformation)
        {
            var order = new Order
            {
                FirstName = userInformation.FirstName,
                LastName = userInformation.LastName,
                Address = userInformation.Address,
                City = userInformation.City,
                Country = userInformation.Country,
                Email = userInformation.Email,
                PhoneNumber = userInformation.PhoneNumber,
                ZipCode = (int)userInformation.ZipCode,
                AditionalInfromation = userInformation.SanitizedInformation,
            };

            foreach (var product in products)
            {
                var targetProduct = await this.db.Products.FirstOrDefaultAsync(x => x.Id == product.Id);
                order.OrderProducts.Add(new OrderProduct
                {
                    ProductId = product.Id,
                    OrderId = order.Id,
                    WantedQuantity = product.WantedQuantity,
                });

                targetProduct.AvailableQuantity =
                    Math.Max(0, product.AvailableQuantity - product.WantedQuantity);
                this.db.Products.Update(targetProduct);
                await this.db.SaveChangesAsync();
            }

            this.db.Orders.Add(order);
            await this.db.SaveChangesAsync();
        }
    }
}