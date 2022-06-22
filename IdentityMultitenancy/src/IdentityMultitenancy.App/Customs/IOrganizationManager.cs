using Microsoft.AspNetCore.Identity;

namespace IdentityMultitenancy.App.Customs
{
    public interface IOrganizationManager : IDisposable
    {
        Task<IdentityResult> CreateAsync(Organization organization);
        Task<IdentityResult> DeleteAsync(Organization organization);
        Task<Organization?> FindByIdAsync(Guid id);
        Task<Organization?> FindByNameAsync(string name);
        Task<IList<Organization>> Get();
        Task<IdentityResult> UpdateAsync(Organization organization);
    }
}