using IdentityMultitenancy.App.Customs;
using IdentityMultitenancy.App.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityMultitenancy.App.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {            
        }

        public DbSet<Organization>? Organizations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("identity");

            var userNameIndex = modelBuilder.Entity<ApplicationUser>().HasIndex(p => new { p.NormalizedUserName }).Metadata;
            modelBuilder.Entity<ApplicationUser>().Metadata.RemoveIndex(userNameIndex.Properties);
            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(p => new { p.NormalizedUserName, p.OrganizationId })
                .HasDatabaseName("UserNameIndex")
                .IsUnique();

            // modelBuilder.Seed();
        }
    }
}