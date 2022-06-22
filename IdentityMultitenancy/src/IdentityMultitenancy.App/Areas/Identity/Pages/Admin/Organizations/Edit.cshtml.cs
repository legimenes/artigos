#nullable disable

using System.ComponentModel.DataAnnotations;
using IdentityMultitenancy.App.Customs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityMultitenancy.App.Areas.Identity.Pages.Admin.Organizations
{
    public class EditModel : PageModel
    {
        private readonly IOrganizationManager _organizationManager;
        private readonly IOrganizationStore _organizationStore;

        public EditModel(IOrganizationManager organizationManager,
            IOrganizationStore organizationStore)
        {
            _organizationManager = organizationManager;
            _organizationStore = organizationStore;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public Guid Id { get; set; }

            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(Guid organizationId)
        {
            if (organizationId == Guid.Empty)
            {
                return NotFound();
            }

            await LoadAsync(organizationId);

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            Organization existingOrganization = await _organizationManager.FindByIdAsync(Input.Id);

            var result = await _organizationManager.DeleteAsync(existingOrganization);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    StatusMessage += error.Description;
                }

                return RedirectToPage(new { organizationId = Input.Id });
            }

            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Organization existingOrganization = await _organizationManager.FindByIdAsync(Input.Id);

            await _organizationStore.SetNameAsync(existingOrganization, Input.Name);

            var result = await _organizationManager.UpdateAsync(existingOrganization);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return Page();
            }

            StatusMessage = "Organization successfully saved.";
            return Page();
        }

        private async Task LoadAsync(Guid organizationId)
        {
            Organization organization = await _organizationManager.FindByIdAsync(organizationId);

            Input = new InputModel
            {
                Id = organization.Id,
                Name = organization.Name
            };
        }
    }
}