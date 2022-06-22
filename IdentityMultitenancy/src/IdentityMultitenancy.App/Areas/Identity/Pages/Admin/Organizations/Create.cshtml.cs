#nullable disable

using System.ComponentModel.DataAnnotations;
using IdentityMultitenancy.App.Customs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityMultitenancy.App.Areas.Identity.Pages.Admin.Organizations
{
    public class CreateModel : PageModel
    {
        private readonly IOrganizationManager _organizationManager;
        private readonly IOrganizationStore _organizationStore;

        public CreateModel(IOrganizationManager organizationManager,
            IOrganizationStore organizationStore)
        {
            _organizationManager = organizationManager;
            _organizationStore = organizationStore;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }
        }

        public IActionResult OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Organization organization = CreateOrganization();

            await _organizationStore.SetNameAsync(organization, Input.Name);

            var result = await _organizationManager.CreateAsync(organization);

            if (result.Succeeded)
            {
                return RedirectToPage("./Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

        private Organization CreateOrganization()
        {
            try
            {
                return Activator.CreateInstance<Organization>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(Organization)}'. " +
                    $"Ensure that '{nameof(Organization)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Admin/Organizations/Create.cshtml");
            }
        }
    }
}