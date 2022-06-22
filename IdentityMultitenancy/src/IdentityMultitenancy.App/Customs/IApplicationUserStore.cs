using Microsoft.AspNetCore.Identity;

namespace IdentityMultitenancy.App.Customs
{
    public interface IApplicationUserStore<TUser> : IUserStore<TUser> where TUser : ApplicationUser
    {
        Task<ApplicationUser?> FindByUserNameAndOrganizationIdAsync(string userName, Guid organizationId, CancellationToken cancellationToken = default);
        Task<Guid> GetOrganizationIdAsync(ApplicationUser user, CancellationToken cancellationToken = default(CancellationToken));
        Task SetOrganizationIdAsync(ApplicationUser user, Guid organizationId, CancellationToken cancellationToken = default(CancellationToken));
    }
}