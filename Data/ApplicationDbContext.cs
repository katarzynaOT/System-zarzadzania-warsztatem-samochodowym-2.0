using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkshopManager.Models;

namespace WorkshopManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
<<<<<<< HEAD
        
=======

>>>>>>> 54dcd2ecc6acc825d8f83c067fbe8d639c7b5495
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<ServiceOrder> ServiceOrders { get; set; }

<<<<<<< HEAD
     
=======

>>>>>>> 54dcd2ecc6acc825d8f83c067fbe8d639c7b5495
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /*builder.Entity<ServiceOrder>()
                   .Property(o => o.Price)
                   .HasPrecision(18, 4);
            */
        }
        public DbSet<WorkshopManager.Models.Comment> Comments { get; set; } = default!;
        public DbSet<WorkshopManager.Models.ServiceTask> ServiceTasks { get; set; } = default!;
        public DbSet<WorkshopManager.Models.UsedPart> UsedParts { get; set; } = default!;
        public DbSet<WorkshopManager.Models.Part> Parts { get; set; } = default!;
    }
}
