using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MultiTenancyIdentity.MVC.Customs;

namespace MultiTenancyIdentity.MVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("identity");

            var userNameIndex = modelBuilder.Entity<ApplicationUser>().HasIndex(p => new { p.NormalizedUserName }).Metadata;
            modelBuilder.Entity<ApplicationUser>().Metadata.RemoveIndex(userNameIndex.Properties);
            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(p => new { p.NormalizedUserName, p.TenantId })
                .HasName("UserNameIndex")
                .IsUnique();
        }
    }
}