// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.Services.Order
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.SdvShop.ViewModels.Order;

    public interface IOrderService
    {
        Task PlaceOrder(ICollection<ProductInCartViewModel> products, OrderInformationViewModel userInformation);

        Task<OrderInformationViewModel> GetUserInformation(string username);
    }
}