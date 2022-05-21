namespace SdvCode.Tests.Tag.Services
{
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.Services.BlogAddons;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class RemoveTagTests
    {
        [Fact]
        public async Task RemoveNoneExistingTag()
        {
            var tag = new Tag { Id = Guid.NewGuid().ToString(), Name = "Test" };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                IBlogAddonsService blodAddonsService = new BlogAddonsService(db);
                db.Tags.Add(tag);
                await db.SaveChangesAsync();
                var result = await blodAddonsService.RemoveTag("testovBug");

                Assert.Equal("Error", result.Item1);
                Assert.Equal(string.Format(ErrorMessages.TagDoesNotExist, "testovBug"), result.Item2);
                Assert.Equal(1, db.Tags.Count());
            }
        }

        [Fact]
        public async Task RemoveTag()
        {
            var tag = new Tag { Id = Guid.NewGuid().ToString(), Name = "Test1" };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                IBlogAddonsService blodAddonsService = new BlogAddonsService(db);
                db.Tags.Add(tag);
                await db.SaveChangesAsync();
                var result = await blodAddonsService.RemoveTag("TeSt1");

                Assert.Equal("Success", result.Item1);
                Assert.Equal(string.Format(SuccessMessages.SuccessfullyRemovedTag, "TeSt1"), result.Item2);
                Assert.Equal(0, db.Tags.Count());
            }
        }
    }
}