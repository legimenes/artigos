using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiTenancyIdentity.MVC.Customs;
using MultiTenancyIdentity.MVC.Data;

[assembly: HostingStartup(typeof(MultiTenancyIdentity.MVC.Areas.Identity.IdentityHostingStartup))]
namespace MultiTenancyIdentity.MVC.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}