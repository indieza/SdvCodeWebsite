// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.Services.ProductComment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.SdvShop.Models;
    using SdvCode.Areas.SdvShop.ViewModels.Comment.InputModels;
    using SdvCode.Areas.SdvShop.ViewModels.User;
    using SdvCode.Data;
    using SdvCode.Models.User;

    public class ProductCommentService : IProductCommentService
    {
        private readonly ApplicationDbContext db;

        public ProductCommentService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task AddComment(AddCommentInputModel model)
        {
            var currentUser = await this.db.Users.FirstOrDefaultAsync(x => x.UserName == model.Username);

            this.db.ProductComments.Add(new ProductComment
            {
                ApplicationUserId = currentUser.Id ?? currentUser.Id,
                Email = model.Email,
                Content = model.SanitizedContent,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                PhoneNumber = model.PhoneNumber,
                ProductId = model.ProductId,
                UserFullName = model.FullName,
            });

            await this.db.SaveChangesAsync();
        }

        public async Task<AddCommentInputModel> ExtractCommentInformation(string username, string productId)
        {
            if (username != null)
            {
                var currentUser = await this.db.Users.FirstOrDefaultAsync(x => x.UserName == username);

                return new AddCommentInputModel
                {
                    Username = currentUser.UserName,
                    Email = currentUser.Email,
                    FullName = $"{currentUser.FirstName} {currentUser.LastName}",
                    PhoneNumber = currentUser.PhoneNumber,
                    Content = string.Empty,
                    ProductId = productId,
                };
            }

            return new AddCommentInputModel();
        }
    }
}