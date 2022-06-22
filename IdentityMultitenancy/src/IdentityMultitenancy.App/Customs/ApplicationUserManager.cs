using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace IdentityMultitenancy.App.Customs
{
    public class ApplicationUserManager<TUser> : UserManager<TUser> where TUser : ApplicationUser
    {
        protected internal IApplicationUserStore<TUser> ApplicationUserStore { get; set; }
        private readonly IServiceProvider _services;

        public ApplicationUserManager(
            IApplicationUserStore<TUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<TUser> passwordHasher,
            IEnumerable<IApplicationUserValidator<TUser>> userValidators,
            IEnumerable<IPasswordValidator<TUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<TUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer,
                errors, services, logger)
        {
            ApplicationUserStore = store;
            _services = services;
            if (services != null)
            {
                foreach (var providerName in Options.Tokens.ProviderMap.Keys)
                {
                    var description = Options.Tokens.ProviderMap[providerName];

                    var provider = (description.ProviderInstance ?? services.GetRequiredService(description.ProviderType))
                        as IUserTwoFactorTokenProvider<TUser>;
                    if (provider != null)
                    {
                        RegisterTokenProvider(providerName, provider);
                    }
                }
            }
        }

        public virtual async Task<TUser?> FindByUserNameAndOrganizationIdAsync(string userName, Guid organizationId)
        {
            ThrowIfDisposed();
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }
            userName = NormalizeName(userName);

            var user = await ApplicationUserStore.FindByUserNameAndOrganizationIdAsync(userName, organizationId, CancellationToken);

            if (user == null && Options.Stores.ProtectPersonalData)
            {
                var keyRing = _services.GetService<ILookupProtectorKeyRing>();
                var protector = _services.GetService<ILookupProtector>();
                if (keyRing != null && protector != null)
                {
                    foreach (var key in keyRing.GetAllKeyIds())
                    {
                        var oldKey = protector.Protect(key, userName);
                        user = await ApplicationUserStore.FindByNameAsync(oldKey, CancellationToken);
                        if (user != null)
                        {
                            return (TUser?)user;
                        }
                    }
                }
            }
            return (TUser?)user;
        }

        public virtual async Task<Guid> GetOrganizationIdAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return await ApplicationUserStore.GetOrganizationIdAsync(user, CancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<IdentityResult> SetOrganizationIdAsync(TUser user, Guid organizationId)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await ApplicationUserStore.SetOrganizationIdAsync(user, organizationId, CancellationToken).ConfigureAwait(false);
            return await UpdateUserAsync(user).ConfigureAwait(false);
        }

        public override async Task<IdentityResult> CreateAsync(TUser user, string password)
        {
            ThrowIfDisposed();

            TUser? existingUser = (TUser?)await ApplicationUserStore.FindByUserNameAndOrganizationIdAsync(user.UserName , user.OrganizationId, CancellationToken);
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError() { Description = "UserName and TenantId already exists" });
            }

            return await base.CreateAsync(user, password);
        }
    }
}