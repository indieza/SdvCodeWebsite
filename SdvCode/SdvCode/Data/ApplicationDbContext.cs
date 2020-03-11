using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SdvCode.Models;

namespace SdvCode.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FollowUnfollow> FollowUnfollows { get; set; }

        public DbSet<UserAction> UserActions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<FollowUnfollow>().HasKey(k => new
            {
                k.PersonId,
                k.FollowerId
            });

            base.OnModelCreating(builder);
        }
    }
}