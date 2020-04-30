// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.SiteReports.BlogReports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.ViewModels.SiteReportsViewModels;
    using SdvCode.Data;
    using Twilio.Rest.Api.V2010.Account.Usage.Record;

    public class BlogPostReport : IBlogPostReport
    {
        private readonly ApplicationDbContext db;
        private readonly List<string> rudeWords = new List<string>
        {
            "Bitch", "Fuck", "Suck", "Fuck yourself", "Suck balls", "Dick", "Fucker", "Gypsy", "Idiot", "Pedal",
            "MotherFucker", "Mother fucker",
        };

        public BlogPostReport(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<ICollection<BlogCommentReportViewModel>> GetCommentsData()
        {
            var comments = this.db.Comments.ToList();
            var result = new List<BlogCommentReportViewModel>();

            foreach (var comment in comments)
            {
                var contentWithoutTags = Regex.Replace(comment.Content, "<.*?>", string.Empty);
                List<string> contentWords = contentWithoutTags.ToLower()
                .Split(new[] { " ", ",", "-", "!", "?", ":", ";" }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
                var targetModel = new BlogCommentReportViewModel
                {
                    Content = contentWithoutTags,
                    CommentStatus = comment.CommentStatus.ToString(),
                    CreatorUsername = (await this.db.Users
                        .FirstOrDefaultAsync(x => x.Id == comment.ApplicationUserId))
                        .UserName,
                    Predicition =
                        contentWords.Any(x => this.rudeWords.Any(y => y.ToLower() == x)) ? "Rude" : string.Empty,
                };
                result.Add(targetModel);
            }

            return result;
        }

        public async Task<ICollection<BlogPostReportViewModel>> GetPostsData()
        {
            var posts = this.db.Posts.ToList();
            var result = new List<BlogPostReportViewModel>();

            foreach (var post in posts)
            {
                var contentWithoutTags = Regex.Replace(post.Content, "<.*?>", string.Empty);
                List<string> contentWords = contentWithoutTags.ToLower()
                .Split(new[] { " ", ",", "-", "!", "?", ":", ";" }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
                var targetModel = new BlogPostReportViewModel
                {
                    Title = post.Title,
                    Description = contentWithoutTags,
                    PostStatus = post.PostStatus.ToString(),
                    Likes = await this.db.PostsLikes.CountAsync(x => x.PostId == post.Id && x.IsLiked == true),
                    Category =
                        (await this.db.Categories.FirstOrDefaultAsync(x => x.Id == post.CategoryId)).Name,
                    CreatorUsername =
                        (await this.db.Users.FirstOrDefaultAsync(x => x.Id == post.ApplicationUserId)).UserName,
                    Predicition =
                        contentWords.Any(x => this.rudeWords.Any(y => y.ToLower() == x)) ? "Rude" : string.Empty,
                };

                var postTags = this.db.PostsTags.Where(x => x.PostId == post.Id).ToList();
                var tagNames = new List<string>();

                foreach (var postTag in postTags)
                {
                    var tag = await this.db.Tags.FirstOrDefaultAsync(x => x.Id == postTag.TagId);
                    tagNames.Add(tag.Name);
                }

                targetModel.TagNames = string.Join(", ", tagNames);
                result.Add(targetModel);
            }

            return result;
        }
    }
}