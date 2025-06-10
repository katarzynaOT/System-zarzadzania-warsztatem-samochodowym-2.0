using Microsoft.AspNetCore.Mvc;

namespace WorkshopManager.Models
{
    public class Part : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
