// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Users.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ManageAccountViewModel
    {
        public ICollection<string> CountryCodes { get; set; } = new HashSet<string>();

        public ICollection<string> Cities { get; set; } = new HashSet<string>();

        public ICollection<string> States { get; set; } = new HashSet<string>();

        public ICollection<string> Countries { get; set; } = new HashSet<string>();
    }
}