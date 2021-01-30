using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MultiTenancyIdentity.MVC.Customs
{
    public interface IApplicationUserStore<TUser> : IUserStore<TUser> where TUser : ApplicationUser
    {
        Task<TUser> FindByUserNameAndTenantIdAsync(string normalizedUserName, long tenantId, CancellationToken cancellationToken);
    }
}