using IdentityMultitenancy.App.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityMultitenancy.App.Customs
{
    public class ApplicationUserStore : UserStore<ApplicationUser, IdentityRole<Guid>, ApplicationDbContext, Guid>, IApplicationUserStore<ApplicationUser>
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserStore(ApplicationDbContext context,
            IdentityErrorDescriber? describer = null)
        : base(context, describer)
        {
            _context = context;
        }

        public async Task<ApplicationUser?> FindByUserNameAndOrganizationIdAsync(string userName, Guid organizationId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return await _context.Users.FirstOrDefaultAsync(u => u.UserName.ToUpper() == userName.ToUpper() && u.OrganizationId == organizationId, cancellationToken);
        }

        public Task<Guid> GetOrganizationIdAsync(ApplicationUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.OrganizationId);
        }

        public Task SetOrganizationIdAsync(ApplicationUser user, Guid organizationId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.OrganizationId = organizationId;
            return Task.CompletedTask;
        }
    }
}