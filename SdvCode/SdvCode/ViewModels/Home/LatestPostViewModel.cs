// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Home
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class LatestPostViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string CreatorUsername { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}