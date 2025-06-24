using Microsoft.AspNetCore.Identity;

namespace WorkshopManager.Models
{
    public class ApplicationUser : IdentityUser
    {
<<<<<<< HEAD
        public string FirstName { get; set; }
        public string LastName { get; set; }
=======
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
>>>>>>> 54dcd2ecc6acc825d8f83c067fbe8d639c7b5495
    }
}
