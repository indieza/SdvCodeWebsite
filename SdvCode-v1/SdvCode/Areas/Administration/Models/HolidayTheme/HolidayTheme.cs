// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Models.HolidayTheme
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class HolidayTheme
    {
        public HolidayTheme()
        {
            this.Id = Guid.NewGuid().ToString();
            this.IsActive = false;
        }

        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        [MaxLength(120)]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public ICollection<HolidayIcon> HolidayIcons { get; set; } = new HashSet<HolidayIcon>();
    }
}