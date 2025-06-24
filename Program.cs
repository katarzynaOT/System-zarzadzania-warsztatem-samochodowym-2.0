using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkshopManager.Data;
using WorkshopManager.Models;
using WorkshopManager.Services;

<<<<<<< HEAD
=======
using QuestPDF;


>>>>>>> 54dcd2ecc6acc825d8f83c067fbe8d639c7b5495
var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
//builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//.AddEntityFrameworkStores<ApplicationDbContext>();
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
//.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

<<<<<<< HEAD
=======
//QuestPDF.Settings.License = LicenseType.Community;
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;


>>>>>>> 54dcd2ecc6acc825d8f83c067fbe8d639c7b5495

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Dodanie rol do systemu
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roleNames = { "Admin", "Mechanik", "Recepcjonista" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Tworzenie domyslnego administratora 
    string adminEmail = "admin@workshop.com";
    string adminPassword = "Admin123!";
    var adminUser = new ApplicationUser
    {
        FirstName = "Admin",
        LastName = "Administrator",
        Email = adminEmail,
        UserName = adminEmail,
    };
    var existingUser = await userManager.FindByEmailAsync(adminEmail);
    if (existingUser == null)
    {
        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }

    // Tworzenie domyslnego recepcjonisty
    string recEmail = "user@workshop.com";
    string recPassword = "User123!";
    var recUser = new ApplicationUser
    {
        FirstName = "Maja",
        LastName = "Wojtowicz",
        Email = recEmail,
        UserName = recEmail,
    };
    existingUser = await userManager.FindByEmailAsync(recEmail);
    if (existingUser == null)
    {
        var result = await userManager.CreateAsync(recUser, recPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(recUser, "Recepcjonista");
        }
    }

    // Tworzenie domyslnych mechanikow
    string mech1Email = "mech1@workshop.com";
    string mech1Password = "Mech123!";
    var mech1User = new ApplicationUser
    {
        FirstName = "Jan",
        LastName = "Mechanik",
        Email = mech1Email,
        UserName = mech1Email,
    };
    existingUser = await userManager.FindByEmailAsync(mech1Email);
    if (existingUser == null)
    {
        var result = await userManager.CreateAsync(mech1User, mech1Password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(mech1User, "Mechanik");
        }
    }

    // Tworzenie domyslnych mechanikow
    string mech2Email = "mech2@workshop.com";
    string mech2Password = "Mech123!";
    var mech2User = new ApplicationUser
    {
        FirstName = "Andrzej",
        LastName = "Elektryk",
        Email = mech2Email,
        UserName = mech2Email,
    };
    existingUser = await userManager.FindByEmailAsync(mech2Email);
    if (existingUser == null)
    {
        var result = await userManager.CreateAsync(mech2User, mech2Password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(mech2User, "Mechanik");
        }
    }

    var services = scope.ServiceProvider;

<<<<<<< HEAD
    SeedData.Initialize(services);
=======
    try
    {
        SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        // Logowanie b³êdów (opcjonalne)
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Wyst¹pi³ b³¹d podczas inicjalizacji bazy danych.");
    }
>>>>>>> 54dcd2ecc6acc825d8f83c067fbe8d639c7b5495

}

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.MapControllers();

app.Run();