using Microsoft.AspNetCore.Identity;

namespace IdentityMultitenancy.App.Customs
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public Guid OrganizationId { get; set; }

        public virtual Organization? Organization { get; set; }
    }
}