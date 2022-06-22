using IdentityMultitenancy.App.Customs;
using IdentityMultitenancy.App.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("IdentityDataContextConnection");;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddUserStore<ApplicationUserStore>()
    .AddUserManager<ApplicationUserManager<ApplicationUser>>()
    .AddSignInManager<ApplicationSignInManager<ApplicationUser>>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IOrganizationStore, OrganizationStore>();
builder.Services.AddScoped<IOrganizationManager, OrganizationManager>();
builder.Services.AddScoped<IApplicationUserStore<ApplicationUser>, ApplicationUserStore>();
builder.Services.AddScoped<IApplicationUserValidator<ApplicationUser>, ApplicationUserValidator<ApplicationUser>>();

builder.Services.AddRazorPages(options => options.Conventions.AuthorizeAreaFolder("Identity", "/Admin"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();

app.Run();
