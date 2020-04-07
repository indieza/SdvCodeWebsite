namespace SdvCode.Tests.Tag.Services
{
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.Services.Tag;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class ExtractPostsByTagIdTests
    {
        [Fact]
        public async Task TestExtractPostsByTagId()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "pesho" };
            var tag = new Tag { Id = Guid.NewGuid().ToString(), Name = "Test Tag" };
            var post = new Post { Id = Guid.NewGuid().ToString(), Title = "Test Title" };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                ITagService tagService = new TagService(db);
                db.Users.AddRange(user);
                db.Tags.Add(tag);
                db.Posts.Add(post);
                db.PostsTags.Add(new PostTag
                {
                    Post = post,
                    Tag = tag,
                });
                db.SaveChanges();
                var result = await tagService.ExtractPostsByTagId(tag.Id, user);

                Assert.Equal(1, result.Count);
            }
        }
    }
}