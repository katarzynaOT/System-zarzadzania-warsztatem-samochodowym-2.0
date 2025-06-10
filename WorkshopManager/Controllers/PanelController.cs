using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WorkshopManager.Controllers
{
    [Authorize]
    public class PanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
