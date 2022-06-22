using Microsoft.AspNetCore.Identity;

namespace IdentityMultitenancy.App.Customs
{
    public class OrganizationManager : IOrganizationManager
    {
        private readonly IOrganizationStore _organizationStore;

        public OrganizationManager(IOrganizationStore organizationStore)
        {
            _organizationStore = organizationStore;
        }

        public async Task<IdentityResult> CreateAsync(Organization organization)
        {
            ThrowIfDisposed();
            var validationResult = await ValidateOrganizationAsync(organization).ConfigureAwait(false);
            if (!validationResult.Succeeded)
            {
                return validationResult;
            }

            return await _organizationStore.CreateAsync(organization, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task<IdentityResult> DeleteAsync(Organization organization)
        {
            ThrowIfDisposed();
            var validationResult = await ValidateDeleteAsync(organization.Id).ConfigureAwait(false);
            if (!validationResult.Succeeded)
            {
                return validationResult;
            }

            return await _organizationStore.DeleteAsync(organization, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task<Organization?> FindByIdAsync(Guid id)
        {
            ThrowIfDisposed();
            return await _organizationStore.FindByIdAsync(id, CancellationToken.None);
        }

        public async Task<Organization?> FindByNameAsync(string name)
        {
            ThrowIfDisposed();
            return await _organizationStore.FindByNameAsync(name, CancellationToken.None);
        }

        public async Task<IList<Organization>> Get()
        {
            ThrowIfDisposed();
            return await _organizationStore.Get(CancellationToken.None);
        }

        public async Task<IdentityResult> UpdateAsync(Organization organization)
        {
            ThrowIfDisposed();
            var validationResult = await ValidateOrganizationAsync(organization).ConfigureAwait(false);
            if (!validationResult.Succeeded)
            {
                return validationResult;
            }

            return await _organizationStore.UpdateAsync(organization, CancellationToken.None).ConfigureAwait(false);
        }

        protected async Task<IdentityResult> ValidateOrganizationAsync(Organization organization)
        {
            var org = await _organizationStore.FindByNameAsync(organization.Name!);

            if (org is not null && org.Id != organization.Id)
            {
                var errors = new List<IdentityError>();
                errors.Add(new() { Description = $"Organization name '{organization.Name}' is already taken." });
                return IdentityResult.Failed(errors.ToArray());
            }

            return IdentityResult.Success;
        }

        protected async Task<IdentityResult> ValidateDeleteAsync(Guid organizationId)
        {
            bool hasUsers = await _organizationStore.HasUsers(organizationId);

            if (hasUsers)
            {
                var errors = new List<IdentityError>();
                errors.Add(new() { Description = $"Organization has user associated." });
                return IdentityResult.Failed(errors.ToArray());
            }

            return IdentityResult.Success;
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
                _organizationStore.Dispose();
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