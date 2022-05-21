// <copyright file="DbContext.cs" company="SDV Code">
// Copyright (c) SDV Code. All rights reserved.
// </copyright>

namespace SdvCode.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using SdvCode.Models;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Image;
    using SdvCode.Models.User;
    using SdvCode.Models.UserInformation;
    using SdvCode.Models.WebsiteActions;
    using SdvCode.Models.WebsiteActions.Post;

    public class Context : IdentityDbContext<User, Role, string,
        IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<BaseData> BaseData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("Users");
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<UserRole>().ToTable("UsersRoles");

            builder.Entity<BaseWebsiteAction>().ToTable("BaseWebsiteActions");
            builder.Entity<BasePostAction>().ToTable("BasePostActions");

            builder.Entity<City>().ToTable("Cities");
            builder.Entity<Country>().ToTable("Countries");
            builder.Entity<CountryCode>().ToTable("CountryCodes");
            builder.Entity<ZipCode>().ToTable("ZipCodes");
            builder.Entity<State>().ToTable("States");

            builder.Entity<Post>().ToTable("Posts");
            builder.Entity<LikeOwnPostAction>().ToTable("LikeOwnPostActions");
            builder.Entity<LikedPostAction>().ToTable("LikedPostActions");
            builder.Entity<LikePostAction>().ToTable("LikePostActions");
            builder.Entity<UnlikeOwnPostAction>().ToTable("UnlikeOwnPostActions");
            builder.Entity<UnlikedPostAction>().ToTable("UnlikedPostActions");
            builder.Entity<UnlikePostAction>().ToTable("UnlikePostActions");

            builder.Entity<BaseImage>().ToTable("BaseImages");

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

            builder.Entity<Country>()
                .HasOne(e => e.CountryCode)
                .WithMany(e => e.Coutries)
                .HasForeignKey(e => e.CountryCodeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>(entity =>
            {
                entity.HasOne(e => e.ZipCode)
                    .WithMany(e => e.ApplicationUsers)
                    .HasForeignKey(e => e.ZipCodeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

                entity.HasOne(e => e.CountryCode)
                    .WithMany(e => e.ApplicationUsers)
                    .HasForeignKey(e => e.CountryCodeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

                entity.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

                entity.HasOne(x => x.State)
                    .WithMany(x => x.ApplicationUsers)
                    .HasForeignKey(x => x.StateId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);
            });

            builder.Entity<Role>(b =>
            {
                b.HasMany(e => e.UsersRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });
        }
    }
}