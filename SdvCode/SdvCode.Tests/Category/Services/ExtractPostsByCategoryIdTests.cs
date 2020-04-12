namespace SdvCode.Tests.Category.Services
{
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.Services.Category;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class ExtractPostsByCategoryIdTests
    {
        [Fact]
        public async Task TestExtractPostsByCategoryId()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "pesho" };
            var category = new Category { Id = Guid.NewGuid().ToString() };
            var post = new Post { Id = Guid.NewGuid().ToString(), Category = category };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                ICategoryService categoryService = new CategoryService(db);
                db.Categories.Add(category);
                db.Posts.Add(post);
                await db.SaveChangesAsync();
                var result = await categoryService.ExtractPostsByCategoryId(category.Id, user);

                Assert.Equal(1, result.Count);
                Assert.Equal(post.Id, result.ToList()[0].Id);
            }
        }
    }
}