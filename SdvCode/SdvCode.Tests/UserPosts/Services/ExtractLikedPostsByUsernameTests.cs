namespace SdvCode.Tests.UserPosts.Services
{
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.Services.UserPosts;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class ExtractLikedPostsByUsernameTests
    {
        [Fact]
        public async Task TestExtractLikedPostsByUsername()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "pesho" };
            var currentUser = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "gogo" };
            var post = new Post { Id = Guid.NewGuid().ToString(), ApplicationUser = user };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                IUserPostsService postService = new UserPostsService(db);
                db.Users.AddRange(user, currentUser);
                db.Posts.Add(post);
                db.PostsLikes.Add(new PostLike
                {
                    ApplicationUser = user,
                    Post = post,
                    IsLiked = true,
                });
                db.SaveChanges();
                var result = await postService.ExtractLikedPostsByUsername(user.UserName, currentUser);

                Assert.Equal(1, result.Count);
            }
        }

        [Fact]
        public async Task TestExtractLikedPostsByUsernameNonePosts()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "pesho" };
            var currentUser = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "gogo" };
            var post = new Post { Id = Guid.NewGuid().ToString(), ApplicationUser = user };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                IUserPostsService postService = new UserPostsService(db);
                db.Users.AddRange(user, currentUser);
                db.Posts.Add(post);
                db.PostsLikes.Add(new PostLike
                {
                    ApplicationUser = user,
                    Post = post,
                    IsLiked = false,
                });
                db.SaveChanges();
                var result = await postService.ExtractLikedPostsByUsername(user.UserName, currentUser);

                Assert.Equal(0, result.Count);
            }
        }
    }
}