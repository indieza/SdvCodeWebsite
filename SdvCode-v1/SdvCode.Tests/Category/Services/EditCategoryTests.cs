namespace SdvCode.Tests.Category.Services
{
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Editor.Services.Category;
    using SdvCode.Areas.Editor.ViewModels;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class EditCategoryTests
    {
        [Fact]
        public async Task EditNoneExistingCategory()
        {
            EditCategoryInputModel model = new EditCategoryInputModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Test1",
                Description = "Test1",
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                IEditCategoryService categoryService = new EditCategoryService(db);
                var result = await categoryService.EditCategory(model);

                Assert.False(result);
            }
        }

        [Fact]
        public async Task EditCategory()
        {
            var category = new Category
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Test",
                Description = "Test",
            };

            EditCategoryInputModel model = new EditCategoryInputModel
            {
                Id = category.Id,
                Name = "Test1",
                Description = "Test1",
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                IEditCategoryService categoryService = new EditCategoryService(db);
                db.Categories.Add(category);
                await db.SaveChangesAsync();
                var result = await categoryService.EditCategory(model);

                Assert.True(result);
                Assert.Equal("Test1", db.Categories.FirstOrDefault(x => x.Id == category.Id).Name);
            }
        }
    }
}