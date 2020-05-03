// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.ShopViewModels.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.SdvShop.Models;

    public class ShopOrderViewModel
    {
        public string Id { get; set; }

        public string UserFullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public int ZipCode { get; set; }

        public string AditionalInformation { get; set; }

        public decimal TotalPrice { get; set; }

        public ICollection<OrderedProductViewModel> Products { get; set; } = new HashSet<OrderedProductViewModel>();
    }
}