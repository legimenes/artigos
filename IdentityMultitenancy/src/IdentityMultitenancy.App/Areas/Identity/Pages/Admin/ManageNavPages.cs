#nullable disable

using Microsoft.AspNetCore.Mvc.Rendering;

namespace  IdentityMultitenancy.App.Areas.Identity.Pages.Admin
{
    public static class ManageNavPages
    {
        public static string Organization => "Organization";

        public static string OrganizationNavClass(ViewContext viewContext) => PageNavClass(viewContext, Organization);

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
