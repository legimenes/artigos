using Microsoft.AspNetCore.Identity;

namespace IdentityMultitenancy.App.Customs
{
    public interface IApplicationUserValidator<TUser> : IUserValidator<TUser> where TUser : ApplicationUser
    {
    }
}