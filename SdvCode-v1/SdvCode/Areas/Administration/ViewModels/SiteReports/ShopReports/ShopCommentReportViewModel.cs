// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.SiteReports.ShopReports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ShopCommentReportViewModel
    {
        public string FullName { get; set; }

        public string Content { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Prediction { get; set; }
    }
}