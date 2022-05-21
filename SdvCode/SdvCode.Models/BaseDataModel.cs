// <copyright file="BaseDataModel.cs" company="SDV Code">
// Copyright (c) SDV Code. All rights reserved.
// </copyright>

namespace SdvCode.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class BaseDataModel
    {
        public BaseDataModel()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedOn = DateTime.UtcNow;
            this.SystemMessage = $"This record was created on {this.CreatedOn.ToLocalTime():dd:MMMM:yyyyy HH:mm:ss}";
        }

        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public string SystemMessage { get; set; }
    }
}