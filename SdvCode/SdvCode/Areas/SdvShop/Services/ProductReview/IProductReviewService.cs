// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.Services.ProductReview
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.SdvShop.ViewModels.Review.InputModels;
    using SdvCode.Areas.SdvShop.ViewModels.Review.ViewModels;

    public interface IProductReviewService
    {
        Task<AddReviewInputModel> ExtractReviewInformation(string username, string productId);

        Task AddReview(AddReviewInputModel inputModel);

        Task<ICollection<ReviewViewModel>> GetAllReviews(string productId);
    }
}