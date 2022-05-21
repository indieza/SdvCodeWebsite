namespace SdvCode.Tests.Comment.Services
{
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Editor.Services.Comment;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class ApprovedCommentByIdTests
    {
        [Fact]
        public async Task ApprovedCommentById()
        {
            var comment = new Comment
            {
                Id = Guid.NewGuid().ToString(),
                Content = "Test1",
                CommentStatus = CommentStatus.Pending
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            //using (var db = new ApplicationDbContext(options))
            //{
            //    IEditorCommentService commentService = new EditorCommentService(db);
            //    db.Comments.AddRange(comment);
            //    await db.SaveChangesAsync();
            //    var result = await commentService.ApprovedCommentById(comment.Id);

            //    Assert.True(result);
            //}
        }

        [Fact]
        public async Task ApprovedCommentByNoneExistingId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            //using (var db = new ApplicationDbContext(options))
            //{
            //    IEditorCommentService commentService = new EditorCommentService(db);
            //    var result = await commentService.ApprovedCommentById(Guid.NewGuid().ToString());

            //    Assert.False(result);
            //}
        }
    }
}