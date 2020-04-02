// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Editor.Services.Post
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IEditorPostService
    {
        Task<bool> ApprovePost(string id);

        Task<bool> BannPost(string id);

        Task<bool> UnbannPost(string id);
    }
}