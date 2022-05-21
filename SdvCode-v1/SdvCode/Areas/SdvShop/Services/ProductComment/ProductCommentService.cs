﻿// Copyright (c) SDV Code Project. All rights reserved.
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
    using SdvCode.Areas.SdvShop.ViewModels.Comment.ViewModel;
    using SdvCode.Areas.SdvShop.ViewModels.User;
    using SdvCode.Constraints;
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
            var comment = new ProductComment
            {
                ApplicationUserId = currentUser?.Id,
                Email = model.Email,
                Content = model.SanitizedContent,
                CreatedOn = DateTime.UtcNow,
                PhoneNumber = model.PhoneNumber,
                ProductId = model.ProductId,
                UserFullName = model.FullName,
                ParentCommentId = model.ParentId,
            };

            await this.db.ProductComments.AddAsync(comment);
            await this.db.SaveChangesAsync();
        }

        public async Task DeleteComment(string id)
        {
            var comment = await this.db.ProductComments.FirstOrDefaultAsync(m => m.Id == id);
            if (comment != null)
            {
                await this.RemoveChildren(comment.Id);
                this.db.ProductComments.Remove(comment);
                await this.db.SaveChangesAsync();
            }
        }

        public async Task<AddCommentInputModel> ExtractCommentInformation(string username, string productId, string parrentId)
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
                    ParentId = parrentId,
                };
            }

            return new AddCommentInputModel
            {
                ProductId = productId,
            };
        }

        public async Task<ICollection<CommentViewModel>> GetAllComments(string productId)
        {
            var comments = this.db.ProductComments
                .Where(x => x.ProductId == productId)
                .ToList();

            var result = new List<CommentViewModel>();

            foreach (var comment in comments)
            {
                var user = await this.db.Users.FirstOrDefaultAsync(x => x.Id == comment.ApplicationUserId);

                result.Add(new CommentViewModel
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    CreatedOn = comment.CreatedOn,
                    UpdatedOn = comment.UpdatedOn,
                    FullName = comment.UserFullName,
                    Username = user?.UserName,
                    ImageUrl = user == null ? GlobalConstants.NoAvatarImageLocation : user.ImageUrl,
                    ProductId = comment.ProductId,
                    ParentId = comment.ParentCommentId,
                });
            }

            return result.OrderBy(x => x.CreatedOn).ToList();
        }

        private async Task RemoveChildren(string i)
        {
            var children = this.db.ProductComments.Where(c => c.ParentCommentId == i).ToList();
            foreach (var child in children)
            {
                await this.RemoveChildren(child.Id);
                this.db.ProductComments.Remove(child);
            }
        }
    }
}