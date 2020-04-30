// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.SiteReports.ShopReports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using SdvCode.Areas.Administration.ViewModels.SiteReportsViewModels.ShopReports;
    using SdvCode.Data;

    public class ShopReport : IShopReport
    {
        private readonly ApplicationDbContext db;
        private readonly List<string> rudeWords = new List<string>
        {
            "Bitch", "Fuck", "Suck", "Fuck yourself", "Suck balls", "Dick", "Fucker", "Gypsy", "Idiot", "Pedal",
            "MotherFucker", "Mother fucker",
        };

        public ShopReport(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ICollection<ShopCommentReportViewModel> GetCommentsData()
        {
            var comments = this.db.ProductComments.ToList();
            var result = new List<ShopCommentReportViewModel>();

            foreach (var comment in comments)
            {
                var contentWithoutTags = Regex.Replace(comment.Content, "<.*?>", string.Empty);
                List<string> contentWords = contentWithoutTags.ToLower()
                .Split(new[] { " ", ",", "-", "!", "?", ":", ";" }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
                var targetModel = new ShopCommentReportViewModel
                {
                    Content = comment.Content,
                    Email = comment.Email,
                    FullName = comment.UserFullName,
                    PhoneNumber = comment.PhoneNumber,
                    Prediction =
                        contentWords.Any(x => this.rudeWords.Any(y => y.ToLower() == x)) ? "Rude" : string.Empty,
                };
                result.Add(targetModel);
            }

            return result;
        }
    }
}