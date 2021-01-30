using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MultiTenancyIdentity.MVC.Data;

namespace MultiTenancyIdentity.MVC.Customs
{
    public class ApplicationUserStore : UserStore<ApplicationUser>, IApplicationUserStore<ApplicationUser>
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserStore(ApplicationDbContext context)
        : base(context)
        {
            _context = context;
        }

        public async Task<ApplicationUser> FindByUserNameAndTenantIdAsync(string userName, long tenantId, CancellationToken cancellationToken = default)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName.ToUpper() == userName.ToUpper() && u.TenantId == tenantId, cancellationToken);
        }
    }
}