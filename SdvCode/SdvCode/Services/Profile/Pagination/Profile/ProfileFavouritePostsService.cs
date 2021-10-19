// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Profile.Pagination.Profile
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using SdvCode.Data;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Post.ViewModels.TopPost;
    using SdvCode.ViewModels.Profile;
    using SdvCode.ViewModels.Profile.UserViewComponents;
    using SdvCode.ViewModels.Profile.UserViewComponents.BlogComponent;

    public class ProfileFavouritePostsService : IProfileFavoritesService
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;

        public ProfileFavouritePostsService(
            ApplicationDbContext db,
            IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public List<FavouritePostViewModel> ExtractFavorites(ApplicationUser user, ApplicationUser currentUser)
        {
            var favorites = this.db.FavouritePosts
                .Where(x => x.ApplicationUserId == user.Id && x.IsFavourite == true)
                .Include(x => x.Post)
                .ThenInclude(x => x.Category)
                .Include(x => x.ApplicationUser)
                .OrderByDescending(x => x.Post.CreatedOn)
                .AsSplitQuery()
                .ToList();

            var model = this.mapper.Map<List<FavouritePostViewModel>>(favorites);
            return model;
        }
    }
}