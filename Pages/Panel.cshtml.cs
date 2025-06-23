using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace WorkshopManager.Pages
{
    [Authorize]
    public class PanelModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
