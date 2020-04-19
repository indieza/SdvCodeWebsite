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

    [Route("api/[controller]")]
    [ApiController]
    public class CommentApiController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody]AddCommentInputModel model)
        {
            var m = model; //ignore this
            return this.Ok();
        }
    }
}