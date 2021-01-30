using Microsoft.AspNetCore.Identity;

namespace MultiTenancyIdentity.MVC.Customs
{
    public class ApplicationUser : IdentityUser {
        public long TenantId { get; set; }
    }
}