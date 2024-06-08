using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CommunityApp.Pages.Leases
{
    [Authorize("AdminOnly")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}