using WorkshopManager.Models;

namespace WorkshopManager.Services
{
    public interface IUserService
    {
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<Dictionary<string, IList<string>>> GetUserRolesAsync(List<ApplicationUser> users);
        Task<bool> ChangeUserRoleAsync(string userId, string newRole);
    }

}
