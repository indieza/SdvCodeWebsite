// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.Services.TrackOrder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MoreLinq.Extensions;
    using SdvCode.Areas.SdvShop.ViewModels.Order;
    using SdvCode.Areas.SdvShop.ViewModels.TrackOrder;
    using SdvCode.Areas.SdvShop.ViewModels.TrackOrder.InputModels;
    using SdvCode.Areas.SdvShop.ViewModels.TrackOrder.ViewModels;

    public interface ITrackOrder
    {
        Task<TrackOrderViewModel> GetOrder(TrackOrderInputModel model);
    }
}