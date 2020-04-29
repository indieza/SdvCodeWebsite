// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.SiteReports.BlogReports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.Administration.ViewModels.SiteReportsViewModels;

    public interface IBlogPostReport
    {
        Task<ICollection<BlogPostReportViewModel>> GetPostsData();
    }
}