// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.PendingComments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.ML;
    using SdvCode.Areas.Administration.ViewModels.PendingComments;
    using SdvCode.MlModels.CommentModels;

    public interface IPendingCommentsService
    {
        Task<ICollection<AdminPendingCommentViewModel>> ExtractAllPendingComments(
            PredictionEnginePool<BlogCommentModelInput, BlogCommentModelOutput> predictionEngine);
    }
}