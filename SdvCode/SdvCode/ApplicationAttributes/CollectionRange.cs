// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ApplicationAttributes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class CollectionRange : ValidationAttribute
    {
        private readonly int minLength;
        private readonly int maxLength;

        public CollectionRange(int minLength, int maxLength)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var collection = value as ICollection;

            if (collection.Count >= this.minLength && collection.Count <= this.maxLength)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult($"Collection should be in range [{this.minLength} - {this.maxLength}].");
        }
    }
}