using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WorkshopManager.Models;
using WorkshopManager.Services;

namespace WorkshopManager.Controllers
{
    [Authorize]
    public class PanelController : Controller
    {
        private readonly IUserService _userService;

        public PanelController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var roles = await _userService.GetUserRolesAsync(users);

            ViewBag.UserRoles = roles;
            return View(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ChangeUserRole(string userId, string newRole)
        {
            if (await _userService.ChangeUserRoleAsync(userId, newRole))
            {
                return RedirectToAction("ManageUsers");
            }
            return BadRequest("Zmiana roli się nie powiodła.");
        }
    }

}
