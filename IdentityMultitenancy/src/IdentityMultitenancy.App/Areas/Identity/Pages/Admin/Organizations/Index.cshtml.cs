using IdentityMultitenancy.App.Customs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityMultitenancy.App.Areas.Identity.Pages.Admin.Organizations
{
    public class IndexModel : PageModel
    {
        private readonly IOrganizationManager _organizationManager;

        public IndexModel(IOrganizationManager organizationManager)
        {
            _organizationManager = organizationManager;
        }

        public IList<Organization> Organizations { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadAsync();
            return Page();
        }

        private async Task LoadAsync()
        {
            Organizations = await _organizationManager.Get();
        }
    }
}