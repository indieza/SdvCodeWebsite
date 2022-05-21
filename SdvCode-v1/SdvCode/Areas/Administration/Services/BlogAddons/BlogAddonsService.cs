// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.BlogAddons
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AngleSharp.Common;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.ViewModels.BlogAddons.ViewModels;
    using SdvCode.Areas.Editor.Services;
    using SdvCode.Areas.Editor.Services.Category;
    using SdvCode.Areas.Editor.ViewModels;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Blog;

    public class BlogAddonsService : AddCategoryService, IBlogAddonsService
    {
        private readonly ApplicationDbContext db;

        public BlogAddonsService(ApplicationDbContext db)
            : base(db)
        {
            this.db = db;
        }

        public async Task<Tuple<string, string>> CreateCategoryAdminArea(string name, string description)
        {
            return await this.CreateCategory(name, description);
        }

        public async Task<Tuple<string, string>> CreateTag(string name)
        {
            if (this.db.Tags.Any(x => x.Name.ToLower() == name.ToLower()))
            {
                return Tuple.Create("Error", string.Format(ErrorMessages.TagAlreadyExist, name));
            }

            var tag = new Tag
            {
                Name = name,
                CreatedOn = DateTime.UtcNow,
            };

            this.db.Tags.Add(tag);
            await this.db.SaveChangesAsync();
            return Tuple.Create("Success", string.Format(SuccessMessages.SuccessfullyAddedTag, name));
        }

        public async Task EditExistingCategory(EditCategoryInputModel model)
        {
            var category = await this.db.Categories.FirstOrDefaultAsync(x => x.Id == model.Id);
            category.Name = model.Name;
            category.Description = model.SanitizedDescription;
            category.UpdatedOn = DateTime.UtcNow;
            this.db.Categories.Update(category);
            await this.db.SaveChangesAsync();
        }

        public ICollection<EditCategoryViewModel> GetAllCategories()
        {
            var categories = this.db.Categories.ToList();
            var result = new List<EditCategoryViewModel>();

            foreach (var category in categories)
            {
                result.Add(new EditCategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                });
            }

            return result;
        }

        public ICollection<string> GetAllTags()
        {
            return this.db.Tags.Select(x => x.Name).OrderBy(x => x).ToList();
        }

        public async Task<GetCategoryDataViewModel> GetCategoryById(string categoryId)
        {
            var targetCategory = await this.db.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);
            return new GetCategoryDataViewModel
            {
                Name = targetCategory.Name,
                Description = targetCategory.Description,
            };
        }

        public async Task<Tuple<string, string>> RemoveTag(string name)
        {
            var targetTag = await this.db.Tags.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
            if (targetTag == null)
            {
                return Tuple.Create("Error", ErrorMessages.TagDoesNotExist);
            }

            this.db.Tags.Remove(targetTag);
            await this.db.SaveChangesAsync();
            return Tuple.Create("Success", string.Format(SuccessMessages.SuccessfullyRemovedTag, name));
        }
    }
}