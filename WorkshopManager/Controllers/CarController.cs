using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Threading.Tasks;
using WorkshopManager.Models;
using WorkshopManager.Data;

namespace WorkshopManager.Controllers
{
    public class CarController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CarController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            
            _env = env;
            Directory.CreateDirectory(Path.Combine(_env.WebRootPath, "uploads"));
        }
        public async Task<IActionResult> Index()
        {
            var vehicles = await _context.Cars.ToListAsync();
            return View(vehicles);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car)
        {
            if (!ModelState.IsValid) return View(car);

            await HandleUploadAsync(car);
            _context.Add(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Car car)
        {
            if (id != car.Id) return NotFound();
            if (!ModelState.IsValid) return View(car);

            var dbCar = await _context.Cars.FindAsync(id);
            if (dbCar == null) return NotFound();

            dbCar.Name = car.Name;
            dbCar.Description = car.Description;

            await HandleUploadAsync(dbCar);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = dbCar.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return NotFound();
            return View(car);
        }

        private async Task HandleUploadAsync(Car target)
        {
            var upload = Request.Form.Files.FirstOrDefault();
            if (upload == null || upload.Length == 0) return;

            var ext = Path.GetExtension(upload.FileName).ToLowerInvariant();
            if (ext is not ".png" and not ".jpg" and not ".jpeg")
            {
                ModelState.AddModelError("upload", "Tylko PNG lub JPG");
                throw new InvalidOperationException("Wrong file type");
            }

            var fileName = $"{Guid.NewGuid()}{ext}";
            var savePath = Path.Combine(_env.WebRootPath, "uploads", fileName);
            await using var fs = new FileStream(savePath, FileMode.Create);
            await upload.CopyToAsync(fs);
            target.imageUrl = $"/uploads/{fileName}";
        }
    }
}

