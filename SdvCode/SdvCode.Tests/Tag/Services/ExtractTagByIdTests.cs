namespace SdvCode.Tests.Tag.Services
{
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Services.Tag;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class ExtractTagByIdTests
    {
        [Fact]
        public async Task TestExtractTagById()
        {
            var tag = new Tag { Id = Guid.NewGuid().ToString(), Name = "Test Tag" };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                ITagService tagService = new TagService(db);
                db.Tags.Add(tag);
                db.SaveChanges();
                var result = await tagService.ExtractTagById(tag.Id);

                Assert.Equal(tag, result);
                Assert.Equal(tag.Id, result.Id);
            }
        }
    }
}