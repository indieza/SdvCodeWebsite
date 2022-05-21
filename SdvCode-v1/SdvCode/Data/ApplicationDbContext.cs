// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using SdvCode.Areas.Administration.Models.HolidayTheme;
    using SdvCode.Areas.PrivateChat.Models;
    using SdvCode.Areas.SdvShop.Models;
    using SdvCode.Areas.UserNotifications.Models;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.Models.User.UserActions;
    using SdvCode.Models.User.UserActions.BlogActions;
    using SdvCode.Models.User.UserActions.ProfileActions;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string,
        IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FollowUnfollow> FollowUnfollows { get; set; }

        public DbSet<UserAction> UserActions { get; set; }

        public DbSet<BaseUserAction> BaseUserActions { get; set; }

        //public DbSet<BaseBlogAction> BaseBlogActions { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<PostImage> PostImages { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<PostTag> PostsTags { get; set; }

        public DbSet<PostLike> PostsLikes { get; set; }

        public DbSet<FavouritePost> FavouritePosts { get; set; }

        public DbSet<PendingPost> PendingPosts { get; set; }

        public DbSet<BlockedPost> BlockedPosts { get; set; }

        public DbSet<ZipCode> ZipCodes { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<CountryCode> CountryCodes { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<UserGroup> UsersGroups { get; set; }

        public DbSet<ChatTheme> ChatThemes { get; set; }

        public DbSet<ChatMessage> ChatMessages { get; set; }

        public DbSet<ChatImage> ChatImages { get; set; }

        public DbSet<RecommendedFriend> RecommendedFriends { get; set; }

        public DbSet<UserRating> UserRatings { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<ProductComment> ProductComments { get; set; }

        public DbSet<ProductReview> ProductReviews { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderProduct> OrderProducts { get; set; }

        public DbSet<UserNotification> UserNotifications { get; set; }

        public DbSet<Emoji> Emojis { get; set; }

        public DbSet<EmojiSkin> EmojiSkins { get; set; }

        public DbSet<StickerType> StickerTypes { get; set; }

        public DbSet<Sticker> Stickers { get; set; }

        public DbSet<FavouriteStickers> FavouriteStickers { get; set; }

        public DbSet<HolidayTheme> HolidayThemes { get; set; }

        public DbSet<HolidayIcon> HolidayIcons { get; set; }

        public DbSet<QuickChatReply> QuickChatReplies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("ApplicationUsers");

            builder.Entity<ApplicationRole>().ToTable("ApplicationRoles");

            builder.Entity<ApplicationUserRole>().ToTable("ApplicationUsersRoles");
            builder.Entity<UnfollowUserAction>().ToTable("UnfollowUserActions");
            builder.Entity<UnfollowedUserAction>().ToTable("UnfollowedUserActions");
            builder.Entity<FollowUserAction>().ToTable("FollowUserActions");
            builder.Entity<FollowedUserAction>().ToTable("FollowedUserActions");
            builder.Entity<ChangeCoverImageUserAction>().ToTable("ChangeCoverImageUserActions");
            builder.Entity<ChangeProfilePictureUserAction>().ToTable("ChangeProfilePictureUserActions");
            builder.Entity<EditPersonalDataUserAction>().ToTable("EditPersonalDataUserActions");

            builder.Entity<CreatePostUserAction>().ToTable("CreatePostUserActions");
            builder.Entity<DeletedPostUserAction>().ToTable("DeletedPostUserActions");
            builder.Entity<DeleteOwnPostUserAction>().ToTable("DeleteOwnPostUserActions");
            builder.Entity<DeletePostUserAction>().ToTable("DeletePostUserActions");
            builder.Entity<EditedPostUserAction>().ToTable("EditedPostUserActions");
            builder.Entity<EditOwnPostUserAction>().ToTable("EditOwnPostUserActions");
            builder.Entity<EditPostUserAction>().ToTable("EditPostUserActions");
            builder.Entity<LikedPostUserAction>().ToTable("LikedPostUserActions");
            builder.Entity<LikeOwnPostUserAction>().ToTable("LikeOwnPostUserActions");
            builder.Entity<LikePostUserAction>().ToTable("LikePostUserActions");
            builder.Entity<UnlikedPostUserAction>().ToTable("UnlikedPostUserActions");
            builder.Entity<UnlikeOwnPostUserAction>().ToTable("UnlikeOwnPostUserActions");
            builder.Entity<UnlikePostUserAction>().ToTable("UnlikePostUserActions");

            builder.Entity<BaseBlogAction>().ToTable("BaseBlogActions");

            builder.Entity<Post>(entity =>
            {
                entity.HasOne(e => e.ApplicationUser)
                    .WithMany(e => e.Posts)
                    .HasForeignKey(e => e.ApplicationUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Comments)
                    .WithOne(e => e.Post)
                    .HasForeignKey(e => e.PostId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Category)
                    .WithMany(e => e.Posts)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.PostImages)
                    .WithOne(e => e.Post)
                    .HasForeignKey(e => e.PostId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<City>(entity =>
            {
                entity.HasOne(e => e.State)
                    .WithMany(e => e.Cities)
                    .HasForeignKey(e => e.StateId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Country)
                    .WithMany(e => e.Cities)
                    .HasForeignKey(e => e.CountryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.ZipCodes)
                    .WithOne(e => e.City)
                    .HasForeignKey(e => e.CityId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.HasOne(e => e.ZipCode)
                    .WithMany(e => e.ApplicationUsers)
                    .HasForeignKey(e => e.ZipCodeId)
                    .IsRequired(false);

                entity.HasOne(e => e.CountryCode)
                    .WithMany(e => e.ApplicationUsers)
                    .HasForeignKey(e => e.CountryCodeId)
                    .IsRequired(false);

                entity.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<ApplicationRole>(b =>
            {
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });

            builder.Entity<FollowUnfollow>(b =>
            {
                b.HasOne(e => e.ApplicationUser)
                    .WithMany(e => e.Followers)
                    .HasForeignKey(e => e.ApplicationUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<FollowUnfollow>(b =>
            {
                b.HasOne(e => e.Follower)
                    .WithMany(e => e.Following)
                    .HasForeignKey(e => e.FollowerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<RecommendedFriend>(b =>
            {
                b.HasOne(e => e.ApplicationUser)
                    .WithMany(e => e.RecommendedFriends)
                    .HasForeignKey(e => e.ApplicationUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<RecommendedFriend>(b =>
            {
                b.HasOne(e => e.RecommendedApplicationUser)
                    .WithMany(e => e.UserRecommendations)
                    .HasForeignKey(e => e.RecommendedApplicationUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<PostTag>().HasKey(k => new
            {
                k.TagId,
                k.PostId,
            });

            builder.Entity<PostLike>().HasKey(k => new
            {
                k.PostId,
                k.UserId,
            });

            builder.Entity<FollowUnfollow>().HasKey(k => new
            {
                k.ApplicationUserId,
                k.FollowerId,
            });

            builder.Entity<RecommendedFriend>().HasKey(k => new
            {
                k.ApplicationUserId,
                k.RecommendedApplicationUserId,
            });

            builder.Entity<FavouritePost>().HasKey(k => new
            {
                k.ApplicationUserId,
                k.PostId,
            });

            builder.Entity<PendingPost>().HasKey(k => new
            {
                k.ApplicationUserId,
                k.PostId,
            });

            builder.Entity<BlockedPost>().HasKey(k => new
            {
                k.ApplicationUserId,
                k.PostId,
            });

            builder.Entity<UserGroup>().HasKey(k => new
            {
                k.GroupId,
                k.ApplicationUserId,
            });

            builder.Entity<UserRating>().HasKey(k => new
            {
                k.Username,
                k.RaterUsername,
            });

            builder.Entity<OrderProduct>().HasKey(k => new
            {
                k.OrderId,
                k.ProductId,
            });

            builder.Entity<FavouriteStickers>().HasKey(k => new
            {
                k.ApplicationUserId,
                k.StickerTypeId,
            });

            builder.Entity<Group>(entity =>
            {
                entity.HasOne(e => e.ChatTheme)
                    .WithMany(e => e.Groups)
                    .HasForeignKey(e => e.ChatThemeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

                entity.HasMany(e => e.ChatImages)
                    .WithOne(e => e.Group)
                    .HasForeignKey(e => e.GroupId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Emoji>()
                .HasMany(e => e.EmojiSkins)
                .WithOne(e => e.Emoji)
                .HasForeignKey(e => e.EmojiId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ChatMessage>()
                .HasMany(e => e.ChatImages)
                .WithOne(e => e.ChatMessage)
                .HasForeignKey(e => e.ChatMessageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Country>()
                .HasOne(e => e.CountryCode)
                .WithMany(e => e.Coutries)
                .HasForeignKey(e => e.CountryCodeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Sticker>()
                .HasOne(e => e.StickerType)
                .WithMany(e => e.Stickers)
                .HasForeignKey(e => e.StickerTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(true);

            builder.Entity<HolidayIcon>()
                .HasOne(e => e.HolidayTheme)
                .WithMany(e => e.HolidayIcons)
                .HasForeignKey(e => e.HolidayThemeId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(true);

            builder.Entity<QuickChatReply>()
                .HasOne(e => e.ApplicationUser)
                .WithMany(e => e.QuickChatReplies)
                .HasForeignKey(e => e.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(true);

            builder.Entity<UserAction>()
               .HasOne(x => x.BaseUserAction)
               .WithOne(x => x.UserAction)
               .HasForeignKey<UserAction>(x => x.BaseUserActionId)
               .IsRequired(true);

            builder.Entity<BaseBlogAction>()
               .HasOne(x => x.BaseUserAction)
               .WithOne(x => x.BaseBlogAction)
               .HasForeignKey<BaseUserAction>(x => x.BaseBlogActionId)
               .IsRequired(false);
        }
    }
}