// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Blog.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CreatePostIndexModel
    {
        public CreatePostInputModel PostInputModel { get; set; }

        public ICollection<string> Categories { get; set; } = new HashSet<string>();

        public ICollection<string> Tags { get; set; } = new HashSet<string>();
    }
}