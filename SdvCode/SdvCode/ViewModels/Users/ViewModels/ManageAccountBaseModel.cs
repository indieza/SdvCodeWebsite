// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Users.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.ViewModels.Users.InputModels;

    public class ManageAccountBaseModel
    {
        public ManageAccountViewModel ManageAccountViewModel { get; set; } = new ManageAccountViewModel();

        public ManageAccountInputModel ManageAccountInputModel { get; set; } = new ManageAccountInputModel();
    }
}