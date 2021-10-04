// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ApplicationAttributes
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.ML.Transforms;

    using SdvCode.ViewModels.Blog.InputModels;

    public class MyTestAttribute : ValidationAttribute
    {
        public MyTestAttribute(int length)
        {
            this.Length = length;
        }

        public int Length { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (CreatePostInputModel)validationContext.ObjectInstance;
            var title = (string)value;

            if (model.Title.Length > this.Length && title.Length > this.Length)
            {
                return new ValidationResult("Error title");
            }

            return ValidationResult.Success;
        }
    }
}