using Microsoft.AspNetCore.Identity;

namespace MultiTenancyIdentity.MVC.Customs
{
    public interface IApplicationUserValidator<TUser> : IUserValidator<TUser> where TUser : ApplicationUser
    {
    }
}