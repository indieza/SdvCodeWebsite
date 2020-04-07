namespace SdvCode.Tests.Category.Services
{
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.Services.Category;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class ExtractCategoryByIdTests
    {
        [Fact]
        public async Task TestExtractCategoryById()
        {
            var category = new Category { Id = Guid.NewGuid().ToString() };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                ICategoryService categoryService = new CategoryService(db);
                db.Categories.Add(category);
                db.SaveChanges();
                var result = await categoryService.ExtractCategoryById(category.Id);

                Assert.Equal(category.Id, result.Id);
            }
        }
    }
}