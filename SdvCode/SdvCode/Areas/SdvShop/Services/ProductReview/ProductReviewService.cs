// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.Services.ProductReview
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.SdvShop.Models;
    using SdvCode.Areas.SdvShop.ViewModels.Review.InputModels;
    using SdvCode.Areas.SdvShop.ViewModels.Review.ViewModels;
    using SdvCode.Constraints;
    using SdvCode.Data;

    public class ProductReviewService : IProductReviewService
    {
        private readonly ApplicationDbContext db;

        public ProductReviewService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task AddReview(AddReviewInputModel inputModel)
        {
            var curretnUser = await this.db.Users.FirstOrDefaultAsync(x => x.UserName == inputModel.Username);
            var review = new ProductReview
            {
                ApplicationUserId = curretnUser?.Id,
                Email = inputModel.Email,
                PhoneNumber = inputModel.PhoneNumber,
                ProductId = inputModel.ProductId,
                CreatedOn = DateTime.UtcNow,
                UserFullName = inputModel.FullName,
                Content = inputModel.SanitizedContent,
                Stars = inputModel.Stars,
            };

            await this.db.ProductReviews.AddAsync(review);
            await this.db.SaveChangesAsync();
        }

        public async Task<ICollection<ReviewViewModel>> GetAllReviews(string productId)
        {
            var reviews = this.db.ProductReviews.Where(x => x.ProductId == productId).ToList();

            var result = new List<ReviewViewModel>();

            foreach (var review in reviews)
            {
                var user = await this.db.Users.FirstOrDefaultAsync(x => x.Id == review.ApplicationUserId);
                result.Add(new ReviewViewModel
                {
                    Id = review.Id,
                    Starts = review.Stars,
                    ProductId = review.ProductId,
                    Content = review.Content,
                    CreatedOn = review.CreatedOn,
                    UpdatedOn = review.UpdatedOn,
                    FullName = review.UserFullName,
                    ImageUrl = user == null ? GlobalConstants.NoAvatarImageLocation : user.ImageUrl,
                    Username = user?.UserName,
                });
            }

            return result.OrderByDescending(x => x.CreatedOn).ToList();
        }

        public async Task<AddReviewInputModel> ExtractReviewInformation(string username, string productId)
        {
            if (username != null)
            {
                var user = await this.db.Users.FirstOrDefaultAsync(x => x.UserName == username);

                return new AddReviewInputModel
                {
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    FullName = $"{user.FirstName} {user.LastName}",
                    ProductId = productId,
                    Username = username,
                    Content = string.Empty,
                };
            }

            return new AddReviewInputModel
            {
                ProductId = productId,
            };
        }

        public ReviewBannerViewModel ExtractReviewsStatistics(ICollection<ReviewViewModel> reviews)
        {
            var sum = reviews.Sum(x => x.Starts);

            return new ReviewBannerViewModel
            {
                ReviewsCount = reviews.Count,
                Rating = sum == 0 ? 0 : sum / (decimal)reviews.Count,
                Stars = new Dictionary<int, int>()
                {
                    { 5, reviews.Count(x => x.Starts == 5) },
                    { 4, reviews.Count(x => x.Starts == 4) },
                    { 3, reviews.Count(x => x.Starts == 3) },
                    { 2, reviews.Count(x => x.Starts == 2) },
                    { 1, reviews.Count(x => x.Starts == 1) },
                },
            };
        }

        public async Task RemoveReview(string id)
        {
            var review = await this.db.ProductReviews.FirstOrDefaultAsync(x => x.Id == id);
            if (review != null)
            {
                this.db.Remove(review);
                await this.db.SaveChangesAsync();
            }
        }
    }
}