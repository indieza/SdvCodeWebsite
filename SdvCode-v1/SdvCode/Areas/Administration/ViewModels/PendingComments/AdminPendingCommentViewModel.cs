﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.PendingComments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;

    public class AdminPendingCommentViewModel
    {
        public Comment Comment { get; set; }

        public ApplicationUser User { get; set; }

        public string MlPrediction { get; set; }

        public decimal MlScore { get; set; }
    }
}