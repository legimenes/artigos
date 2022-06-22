using IdentityMultitenancy.App.Customs;
using Microsoft.EntityFrameworkCore;

namespace IdentityMultitenancy.App.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Organization>().HasData(
                new Organization
                {
                    Id = Guid.NewGuid(),
                    Name = "Master"
                }
            );
        }
    }
}