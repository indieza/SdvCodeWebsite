// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.DbUsage
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Enums;
    using SdvCode.Services;
    using SdvCode.Services.Cloud;

    public class DbUsageService : IDbUsageService
    {
        private readonly ApplicationDbContext db;
        private readonly Cloudinary cloudinary;

        public DbUsageService(
            ApplicationDbContext db,
            Cloudinary cloudinary)
        {
            this.db = db;
            this.cloudinary = cloudinary;
        }

        public async Task<int> DeleteAllUsersImages()
        {
            int count = 0;
            var users = this.db.Users.Where(x => x.ImageUrl != null || x.CoverImageUrl != null).ToList();

            foreach (var user in users)
            {
                if (user.ImageUrl != null)
                {
                    user.ImageUrl = null;
                    ApplicationCloudinary.DeleteImage(
                        this.cloudinary,
                        string.Format(GlobalConstants.CloudinaryUserProfilePictureName, user.UserName));
                    count++;
                }

                if (user.CoverImageUrl != null)
                {
                    user.CoverImageUrl = null;
                    ApplicationCloudinary.DeleteImage(
                        this.cloudinary,
                        string.Format(GlobalConstants.CloudinaryUserCoverImageName, user.UserName));
                    count++;
                }
            }

            this.db.Users.UpdateRange(users);
            await this.db.SaveChangesAsync();
            return count;
        }

        public async Task<bool> DeleteUserImagesByUsername(string username)
        {
            var user = this.db.Users.FirstOrDefault(x => x.UserName == username);

            if (user != null && (user.ImageUrl != null || user.CoverImageUrl != null))
            {
                user.ImageUrl = null;
                ApplicationCloudinary.DeleteImage(
                    this.cloudinary,
                    string.Format(GlobalConstants.CloudinaryUserProfilePictureName, username));

                user.CoverImageUrl = null;
                ApplicationCloudinary.DeleteImage(
                    this.cloudinary,
                    string.Format(GlobalConstants.CloudinaryUserCoverImageName, username));

                this.db.Users.Update(user);
                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public ICollection<string> GetAllUsernames()
        {
            return this.db.Users.Select(x => x.UserName).ToList();
        }

        public async Task<bool> RemoveActivitiesByName(UserActionsType actionValue)
        {
            var allActions = this.db.UserActions.Where(x => x.Action == actionValue).ToList();

            if (allActions.Count() == 0)
            {
                return false;
            }

            this.db.UserActions.RemoveRange(allActions);
            await this.db.SaveChangesAsync();
            return true;
        }

        public async Task<int> RemoveAllActivities()
        {
            var allActivities = this.db.UserActions.ToList();
            int count = allActivities.Count();

            if (count > 0)
            {
                this.db.UserActions.RemoveRange(allActivities);
                await this.db.SaveChangesAsync();
            }

            return count;
        }
    }
}