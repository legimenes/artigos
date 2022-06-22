using IdentityMultitenancy.App.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityMultitenancy.App.Customs
{
    public class OrganizationStore : IOrganizationStore
    {
        private readonly ApplicationDbContext _context;

        public OrganizationStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool AutoSaveChanges { get; set; } = true;

        public async Task<IdentityResult> CreateAsync(Organization organization, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (organization == null)
            {
                throw new ArgumentNullException(nameof(organization));
            }
            _context.Add(organization);
            await SaveChanges(cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(Organization organization, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (organization == null)
            {
                throw new ArgumentNullException(nameof(organization));
            }
            _context.Remove(organization);
            await SaveChanges(cancellationToken);
            return IdentityResult.Success;
        }

        public Task<Organization?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return _context.Organizations!.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public Task<Organization?> FindByNameAsync(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return _context.Organizations!.FirstOrDefaultAsync(p => p.Name == name, cancellationToken);
        }

        public async Task<IList<Organization>> Get(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return await _context.Organizations!.OrderBy(p => p.Name).ToListAsync();
        }

        public async Task<bool> HasUsers(Guid organizationId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return await _context.Users.AnyAsync(u => u.OrganizationId == organizationId);
        }

        public Task SetNameAsync(Organization organization, string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (organization == null)
            {
                throw new ArgumentNullException(nameof(organization));
            }
            organization.Name = name;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(Organization organization, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (organization == null)
            {
                throw new ArgumentNullException(nameof(organization));
            }
            _context.Attach(organization);
            await SaveChanges(cancellationToken);
            return IdentityResult.Success;
        }

        protected Task SaveChanges(CancellationToken cancellationToken)
        {
            return AutoSaveChanges ? _context.SaveChangesAsync(cancellationToken) : Task.CompletedTask;
        }

        #region IDisposable
        
        private bool _disposed;

        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        protected void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _context?.Dispose();
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}