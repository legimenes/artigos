// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using IdentityMultitenancy.App.Customs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityMultitenancy.App.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        //private readonly UserManager<IdentityUser> _userManager;
        //private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationSignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationUserManager<ApplicationUser> _userManager;
        private readonly IOrganizationManager _organizationManager;

        public IndexModel(
            ApplicationUserManager<ApplicationUser> userManager, //UserManager<IdentityUser> userManager,
            ApplicationSignInManager<ApplicationUser> signInManager, //SignInManager<IdentityUser> signInManager)
            IOrganizationManager organizationManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _organizationManager = organizationManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name = "Organization")]
            public Guid SelectedOrganization { get; set; }
            public SelectList Organizations { get; set; }
        }

        //private async Task LoadAsync(IdentityUser user)
        private async Task LoadAsync(ApplicationUser user)
        {
            IList<Organization> organizations = await _organizationManager.Get();

            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var organizationId = await _userManager.GetOrganizationIdAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                SelectedOrganization = organizationId,
                Organizations = new SelectList(organizations, nameof(Organization.Id), nameof(Organization.Name))
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            var organizationId = await _userManager.GetOrganizationIdAsync(user);
            if (Input.SelectedOrganization != organizationId)
            {
                var setOrganizationIdResult = await _userManager.SetOrganizationIdAsync(user, Input.SelectedOrganization);
                if (!setOrganizationIdResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set organization.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
