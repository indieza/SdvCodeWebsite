// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.Models.HolidayTheme;
    using SdvCode.Areas.PrivateChat.Models;
    using SdvCode.Areas.SdvShop.Models;
    using SdvCode.Areas.UserNotifications.Models;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FollowUnfollow> FollowUnfollows { get; set; }

        public DbSet<UserAction> UserActions { get; set; }

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
            builder.Entity<Post>(entity =>
            {
                entity.HasOne(x => x.ApplicationUser)
                    .WithMany(x => x.Posts)
                    .HasForeignKey(x => x.ApplicationUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(x => x.Comments)
                    .WithOne(x => x.Post)
                    .HasForeignKey(x => x.PostId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Category)
                    .WithMany(x => x.Posts)
                    .HasForeignKey(x => x.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(x => x.PostImages)
                    .WithOne(x => x.Post)
                    .HasForeignKey(x => x.PostId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<City>(entity =>
            {
                entity.HasOne(x => x.State)
                    .WithMany(x => x.Cities)
                    .HasForeignKey(x => x.StateId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Country)
                    .WithMany(x => x.Cities)
                    .HasForeignKey(x => x.CountryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(x => x.ZipCodes)
                    .WithOne(x => x.City)
                    .HasForeignKey(x => x.CityId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.HasOne(x => x.ZipCode)
                    .WithMany(x => x.ApplicationUsers)
                    .HasForeignKey(x => x.ZipCodeId)
                    .IsRequired(false);

                entity.HasOne(x => x.CountryCode)
                    .WithMany(x => x.ApplicationUsers)
                    .HasForeignKey(x => x.CountryCodeId)
                    .IsRequired(false);
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
                k.PersonId,
                k.FollowerId,
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
                entity.HasOne(x => x.ChatTheme)
                    .WithMany(x => x.Groups)
                    .HasForeignKey(x => x.ChatThemeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

                entity.HasMany(x => x.ChatImages)
                    .WithOne(x => x.Group)
                    .HasForeignKey(x => x.GroupId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Emoji>()
                .HasMany(x => x.EmojiSkins)
                .WithOne(x => x.Emoji)
                .HasForeignKey(x => x.EmojiId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ChatMessage>()
                .HasMany(x => x.ChatImages)
                .WithOne(x => x.ChatMessage)
                .HasForeignKey(x => x.ChatMessageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Country>()
                .HasOne(x => x.CountryCode)
                .WithMany(x => x.Coutries)
                .HasForeignKey(x => x.CountryCodeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Sticker>()
                .HasOne(x => x.StickerType)
                .WithMany(x => x.Stickers)
                .HasForeignKey(x => x.StickerTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(true);

            builder.Entity<HolidayIcon>()
                .HasOne(x => x.HolidayTheme)
                .WithMany(x => x.HolidayIcons)
                .HasForeignKey(x => x.HolidayThemeId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(true);

            builder.Entity<QuickChatReply>()
                .HasOne(x => x.ApplicationUser)
                .WithMany(x => x.QuickChatReplies)
                .HasForeignKey(x => x.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(true);

            base.OnModelCreating(builder);
        }
    }
}