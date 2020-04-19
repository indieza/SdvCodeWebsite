// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.Services.ProductComment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using SdvCode.Areas.SdvShop.ViewModels.User;
    using SdvCode.Models.User;

    public class ProductCommentService : IProductCommentService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ProductCommentService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<UserViewModelForComment> ExtractUserInformation(string username)
        {
            if (username != null)
            {
                var currentUser = await this.userManager.FindByNameAsync(username);

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