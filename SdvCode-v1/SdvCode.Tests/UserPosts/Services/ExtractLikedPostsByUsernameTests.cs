namespace SdvCode.Tests.UserPosts.Services
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Moq;

    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
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
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                       new Mock<IUserStore<ApplicationUser>>().Object,
                       new Mock<IOptions<IdentityOptions>>().Object,
                       new Mock<IPasswordHasher<ApplicationUser>>().Object,
                       new IUserValidator<ApplicationUser>[0],
                       new IPasswordValidator<ApplicationUser>[0],
                       new Mock<ILookupNormalizer>().Object,
                       new Mock<IdentityErrorDescriber>().Object,
                       new Mock<IServiceProvider>().Object,
                       new Mock<ILogger<UserManager<ApplicationUser>>>().Object);
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "pesho" };
            var currentUser = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "gogo" };
            var post = new Post { Id = Guid.NewGuid().ToString(), ApplicationUser = user, PostStatus = PostStatus.Approved };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                //IUserPostsService postService = new UserPostsService(db, mockUserManager.Object);
                //db.Users.AddRange(user, currentUser);
                //db.Posts.Add(post);
                //db.PostsLikes.Add(new PostLike
                //{
                //    ApplicationUser = user,
                //    Post = post,
                //    IsLiked = true,
                //});
                //await db.SaveChangesAsync();
                //var result = await postService.ExtractLikedPostsByUsername(user.UserName, currentUser);

                //Assert.Equal(1, result.Count);
            }
        }

        [Fact]
        public async Task TestExtractLikedPostsByUsernameNonePosts()
        {
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                       new Mock<IUserStore<ApplicationUser>>().Object,
                       new Mock<IOptions<IdentityOptions>>().Object,
                       new Mock<IPasswordHasher<ApplicationUser>>().Object,
                       new IUserValidator<ApplicationUser>[0],
                       new IPasswordValidator<ApplicationUser>[0],
                       new Mock<ILookupNormalizer>().Object,
                       new Mock<IdentityErrorDescriber>().Object,
                       new Mock<IServiceProvider>().Object,
                       new Mock<ILogger<UserManager<ApplicationUser>>>().Object);
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "pesho" };
            var currentUser = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "gogo" };
            var post = new Post { Id = Guid.NewGuid().ToString(), ApplicationUser = user };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                //IUserPostsService postService = new UserPostsService(db, mockUserManager.Object);
                //db.Users.AddRange(user, currentUser);
                //db.Posts.Add(post);
                //db.PostsLikes.Add(new PostLike
                //{
                //    ApplicationUser = user,
                //    Post = post,
                //    IsLiked = false,
                //});
                //await db.SaveChangesAsync();
                //var result = await postService.ExtractLikedPostsByUsername(user.UserName, currentUser);

                //Assert.Equal(0, result.Count);
            }
        }
    }
}