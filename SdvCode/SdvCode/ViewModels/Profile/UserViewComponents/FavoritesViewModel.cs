// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Profile.UserViewComponents
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class FavoritesViewModel
    {
        public string PostId { get; set; }

        public string PostTitle { get; set; }

        public string PostContent { get; set; }

        public bool IsFavorite { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}