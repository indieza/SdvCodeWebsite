﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.Services.ProductComment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.SdvShop.ViewModels.Comment.InputModels;
    using SdvCode.Areas.SdvShop.ViewModels.Comment.ViewModel;
    using SdvCode.Areas.SdvShop.ViewModels.User;

    public interface IProductCommentService
    {
        Task<AddCommentInputModel> ExtractCommentInformation(string username, string productId, string parrentId);

        Task AddComment(AddCommentInputModel model);

        Task<ICollection<CommentViewModel>> GetAllComments(string productId);

        Task DeleteComment(string id);
    }
}