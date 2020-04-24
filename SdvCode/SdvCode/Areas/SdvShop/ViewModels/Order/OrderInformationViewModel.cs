// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.ViewModels.Order
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Ganss.XSS;

    public class OrderInformationViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(15)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(15)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(30)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(100)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [MaxLength(20)]
        public string Country { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [MaxLength(20)]
        public string City { get; set; }

        [Required(ErrorMessage = "Zip code is required.")]
        public int? ZipCode { get; set; }

        public string AditionalInfromation { get; set; }

        public string SanitizedInformation => new HtmlSanitizer().Sanitize(this.AditionalInfromation);
    }
}