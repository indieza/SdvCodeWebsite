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

        public async Task<UserViewModelForComment> ExtractUserInformation(string username)
        {
            if (username != null)
            {
                var currentUser = await this.db.Users.FirstOrDefaultAsync(x => x.UserName == username);

                return new UserViewModelForComment
                {
                    Id = currentUser.Id,
                    Username = currentUser.UserName,
                    Email = currentUser.Email,
                    FullName = $"{currentUser.FirstName} {currentUser.LastName}",
                    PhoneNumber = currentUser.PhoneNumber,
                };
            }

            return new UserViewModelForComment();
        }
    }
}