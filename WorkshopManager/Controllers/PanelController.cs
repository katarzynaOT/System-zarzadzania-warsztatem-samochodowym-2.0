using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WorkshopManager.Models;

namespace WorkshopManager.Controllers
{
    [Authorize]
    public class PanelController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PanelController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageUsers()
        {
            Console.WriteLine("Uruchomiono ManageUsers!");
            var users = _userManager.Users.ToList();
            var userRoles = new Dictionary<string, IList<string>>();

            foreach (var user in users)
            {
                userRoles[user.Id] = await _userManager.GetRolesAsync(user);
            }

            ViewBag.UserRoles = userRoles;
            return View(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ChangeUserRole([FromForm] string userId, [FromForm] string newRole)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(newRole))
            {
                return BadRequest("UserId i NewRole są wymagane.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {           
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (currentRoles.Any())
                {
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                }

                var result = await _userManager.AddToRoleAsync(user, newRole);
                if (!result.Succeeded)
                {
                    return BadRequest("Nie udało się przypisać roli.");
                }
                return RedirectToAction("ManageUsers"); 
            }
            return NotFound("Użytkownik nie został znaleziony.");
        }
    }
}
