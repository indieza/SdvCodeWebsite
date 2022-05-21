// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.PendingPosts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.ML;
    using SdvCode.Areas.Administration.ViewModels.PendingPosts;
    using SdvCode.Data;
    using SdvCode.MlModels.PostModels;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;

    public class PendingPostsService : IPendingPostsService
    {
        private readonly ApplicationDbContext db;

        public PendingPostsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<ICollection<AdminPendingPostViewModel>> ExtractAllPendingPosts(
            PredictionEnginePool<BlogPostModelInput, BlogPostModelOutput> predictionEngine)
        {
            var pendingPosts = this.db.Posts.Where(x => x.PostStatus == PostStatus.Pending).ToList();
            List<AdminPendingPostViewModel> model = new List<AdminPendingPostViewModel>();

            foreach (var post in pendingPosts)
            {
                var contentWithoutTags = Regex.Replace(post.Content, "<.*?>", string.Empty);
                var prediction = predictionEngine.Predict(new BlogPostModelInput
                {
                    Content = contentWithoutTags,
                });

                model.Add(new AdminPendingPostViewModel
                {
                    Post = post,
                    User = await this.db.Users.FirstOrDefaultAsync(x => x.Id == post.ApplicationUserId),
                    MlPrediction = prediction.Prediction,
                    MlScore = (decimal)prediction.Score[0],
                });
            }

            return model;
        }
    }
}