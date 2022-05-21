// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.PendingComments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.ML;
    using SdvCode.Areas.Administration.ViewModels.PendingComments;
    using SdvCode.Data;
    using SdvCode.MlModels.CommentModels;
    using SdvCode.Models.Enums;

    public class PendingCommentsService : IPendingCommentsService
    {
        private readonly ApplicationDbContext db;

        public PendingCommentsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<ICollection<AdminPendingCommentViewModel>> ExtractAllPendingComments(
            PredictionEnginePool<BlogCommentModelInput, BlogCommentModelOutput> predictionEngine)
        {
            var pendingComments = this.db.Comments.Where(x => x.CommentStatus == CommentStatus.Pending).ToList();
            List<AdminPendingCommentViewModel> model = new List<AdminPendingCommentViewModel>();

            foreach (var comment in pendingComments)
            {
                var contentWithoutTags = Regex.Replace(comment.Content, "<.*?>", string.Empty);
                var prediction = predictionEngine.Predict(new BlogCommentModelInput
                {
                    Content = contentWithoutTags,
                });

                var targetComment = new AdminPendingCommentViewModel
                {
                    Comment = comment,
                    User = await this.db.Users.FirstOrDefaultAsync(x => x.Id == comment.ApplicationUserId),
                    MlPrediction = prediction.Prediction,
                    MlScore = (decimal)prediction.Score[0],
                };

                targetComment.Comment.Post = await this.db.Posts.FirstOrDefaultAsync(x => x.Id == comment.PostId);
                model.Add(targetComment);
            }

            return model;
        }
    }
}