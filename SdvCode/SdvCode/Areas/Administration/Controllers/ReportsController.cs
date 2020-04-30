// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OfficeOpenXml;
    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Areas.Administration.Services.SiteReports.BlogReports;
    using SdvCode.Areas.Administration.Services.SiteReports.ShopReports;
    using SdvCode.Areas.Administration.ViewModels.SiteReportsViewModels;
    using SdvCode.Areas.Administration.ViewModels.SiteReportsViewModels.BlogReports;
    using SdvCode.Areas.Administration.ViewModels.SiteReportsViewModels.ShopReports;
    using SdvCode.Constraints;
    using SdvCode.Models.Blog;

    [Area(GlobalConstants.AdministrationArea)]
    public class ReportsController : Controller
    {
        private readonly IBlogPostReport blogPostReport;
        private readonly IShopReport shopReport;

        public ReportsController(IBlogPostReport blogPostReport, IShopReport shopReport)
        {
            this.blogPostReport = blogPostReport;
            this.shopReport = shopReport;
        }

        [Authorize(Roles = GlobalConstants.AdministratorRole)]
        public async Task<IActionResult> BlogPostsReport()
        {
            ICollection<BlogPostReportViewModel> posts = await this.blogPostReport.GetPostsData();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(posts, true);
                package.Save();
            }

            stream.Position = 0;
            string excelName = $"Blog Posts Report - {DateTime.Now:dd-MMMM-yyyy}.xlsx";
            return this.File(
                stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                excelName);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRole)]
        public async Task<IActionResult> BlogCommentsReport()
        {
            ICollection<BlogCommentReportViewModel> comments = await this.blogPostReport.GetCommentsData();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(comments, true);
                package.Save();
            }

            stream.Position = 0;
            string excelName = $"Blog Comments Report - {DateTime.Now:dd-MMMM-yyyy}.xlsx";
            return this.File(
                stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                excelName);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRole)]
        public IActionResult ShopCommentsReport()
        {
            ICollection<ShopCommentReportViewModel> comments = this.shopReport.GetCommentsData();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(comments, true);
                package.Save();
            }

            stream.Position = 0;
            string excelName = $"Shop Comments Report - {DateTime.Now:dd-MMMM-yyyy}.xlsx";
            return this.File(
                stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                excelName);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRole)]
        public IActionResult ShopReviewsReport()
        {
            ICollection<ShopReviewReportViewModel> comments = this.shopReport.GetReviewsData();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(comments, true);
                package.Save();
            }

            stream.Position = 0;
            string excelName = $"Shop Reviews Report - {DateTime.Now:dd-MMMM-yyyy}.xlsx";
            return this.File(
                stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                excelName);
        }
    }
}