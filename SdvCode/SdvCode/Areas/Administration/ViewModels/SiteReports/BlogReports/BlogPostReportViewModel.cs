// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.SiteReports.BlogReports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class BlogPostReportViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int Likes { get; set; }

        public string Category { get; set; }

        public string TagNames { get; set; }

        public string CreatorUsername { get; set; }

        public string PostStatus { get; set; }

        public string Predicition { get; set; }
    }
}