using Microsoft.AspNetCore.Identity;

namespace IdentityMultitenancy.App.Customs
{
    public interface IOrganizationStore : IDisposable
    {
        Task<IdentityResult> CreateAsync(Organization organization, CancellationToken cancellationToken = default(CancellationToken));
        Task<IdentityResult> DeleteAsync(Organization organization, CancellationToken cancellationToken = default(CancellationToken));
        Task<Organization?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
        Task<Organization?> FindByNameAsync(string name, CancellationToken cancellationToken = default(CancellationToken));
        Task<IList<Organization>> Get(CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> HasUsers(Guid organizationId, CancellationToken cancellationToken = default(CancellationToken));
        Task SetNameAsync(Organization organization, string name, CancellationToken cancellationToken = default(CancellationToken));
        Task<IdentityResult> UpdateAsync(Organization organization, CancellationToken cancellationToken = default(CancellationToken));
    }
}