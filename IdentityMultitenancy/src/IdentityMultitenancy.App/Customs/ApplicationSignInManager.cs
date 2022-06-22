using IdentityMultitenancy.App.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace IdentityMultitenancy.App.Customs
{
    public class ApplicationSignInManager<TUser> : SignInManager<TUser> where TUser : ApplicationUser
    {
        private readonly ApplicationUserManager<TUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _contextAccessor;

        public ApplicationSignInManager(
            ApplicationUserManager<TUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<TUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<TUser>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<TUser> confirmation,
            ApplicationDbContext dbContext)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
            if (userManager == null)
                throw new ArgumentNullException(nameof(userManager));

            if (dbContext == null)
                throw new ArgumentNullException(nameof(dbContext));

            if (contextAccessor == null)
                throw new ArgumentNullException(nameof(contextAccessor));

            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _dbContext = dbContext;
        }

        public virtual async Task<SignInResult> PasswordSignInAsync(string userName, string password, Guid organizationId, bool isPersistent, bool lockoutOnFailure)
        {
            ApplicationUser? user = await _userManager.FindByUserNameAndOrganizationIdAsync(userName, organizationId);
            if (user == null)
            {
                return SignInResult.Failed;
            }

            return await PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
        }
    }
}