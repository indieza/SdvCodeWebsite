// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Areas.SdvShop.ViewModels.Comment.InputModels;
    using SdvCode.Constraints;

    [Area(GlobalConstants.ShopArea)]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentApiController : ControllerBase
    {
        [HttpPost]
        public async Task AddComment(AddCommentInputModel model)
        {
            var m = model;
        }
    }
}