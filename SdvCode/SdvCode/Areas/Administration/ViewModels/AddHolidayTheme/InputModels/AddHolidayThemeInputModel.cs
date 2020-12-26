// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.AddHolidayTheme.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using SdvCode.ApplicationAttributes;

    public class AddHolidayThemeInputModel
    {
        [Required]
        [MaxLength(120)]
        public string Name { get; set; }

        [Required]
        [FilesCollectionRange(1, 14)]
        public ICollection<IFormFile> Icons { get; set; } = new HashSet<IFormFile>();
    }
}