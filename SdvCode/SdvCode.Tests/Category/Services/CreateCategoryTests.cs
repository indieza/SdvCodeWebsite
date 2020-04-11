namespace SdvCode.Tests.Category.Services
{
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Editor.Services.Category;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class CreateCategoryTests
    {
        [Fact]
        public async Task CreateExistingCategory()
        {
            var category = new Category { Id = Guid.NewGuid().ToString(), Name = "Test1" };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                IAddCategoryService categoryService = new AddCategoryService(db);
                db.Categories.AddRange(category);
                db.SaveChanges();
                var result = await categoryService.CreateCategory("Test1", "Test");

                Assert.Equal(1, db.Categories.Count());
                Assert.Equal("Error", result.Item1);
                Assert.Equal(string.Format(ErrorMessages.CategoryAlreadyExist, "Test1"), result.Item2);
            }
        }

        [Fact]
        public async Task CreateCategory()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                IAddCategoryService categoryService = new AddCategoryService(db);
                var result = await categoryService.CreateCategory("Test1", "Test");

                Assert.Equal(1, db.Categories.Count());
                Assert.Equal("Success", result.Item1);
                Assert.Equal(string.Format(SuccessMessages.SuccessfullyAddedCategory, "Test1"), result.Item2);
            }
        }
    }
}