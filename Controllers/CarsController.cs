using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using WorkshopManager.Data;
using WorkshopManager.Models;

namespace WorkshopManager.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;


        public CarsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;

            _env = env;
            Directory.CreateDirectory(Path.Combine(_env.WebRootPath, "uploads"));

        }

        // GET: Cars
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Cars.Include(c => c.Customer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        public IActionResult Create(int? customer_id)
        {
            //https://stackoverflow.com/questions/12727285/mvc-selectlist-combining-multiple-columns-in-text-field
            if (customer_id != null)
            {
                System.Diagnostics.Debug.WriteLine("Passed customer id to car create:" + customer_id);
                var customerTable = _context.Customers
                .Where (s=> s.Id == customer_id)
                    .Select(s => new
                {
                    Id = s.Id,
                    CustomerDescription = string.Format("{0} {1}", s.Name, s.LastName)
                }).ToList();
                ViewData["CustomerId"] = new SelectList(customerTable, "Id", "CustomerDescription");
                //ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id");
                //System.Diagnostics.Debug.WriteLine("Creating car - 1p2");
                return View();

            }
            else
            {
                //System.Diagnostics.Debug.WriteLine("Creating car - 1p1");
                var customerTable = _context.Customers
                .Select(s => new
                {
                    Id = s.Id,
                    CustomerDescription = string.Format("{0} {1}", s.Name, s.LastName)
                }).ToList();
                ViewData["CustomerId"] = new SelectList(customerTable, "Id", "CustomerDescription");
                //ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id");
                //System.Diagnostics.Debug.WriteLine("Creating car - 1p2");
                return View();
            }
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerId,Name,Description,Brand,Model,VIN,RegistrationPlate,ManufacturedYear,imageUrl")] Car car)
        //public async Task<IActionResult> Create(Car car)
        {
            //System.Diagnostics.Debug.WriteLine("Creating car - p1");
            var customerTable = _context.Customers
            .Select(s => new
             {   
                Id = s.Id,
                CustomerDescription = string.Format("{0} {1}", s.Name, s.LastName)
            }).ToList();
            ViewData["CustomerId"] = new SelectList(customerTable, "Id", "CustomerDescription");
            //ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id");
            //System.Diagnostics.Debug.WriteLine("Creating car - p2");
            //if (ModelState.IsValid)
            //{
                //System.Diagnostics.Debug.WriteLine("Creating car - p3");
                await HandleUploadAsync(car);
                //System.Diagnostics.Debug.WriteLine("Creating car - p4");
                _context.Add(car);
                //System.Diagnostics.Debug.WriteLine("Creating car - p5");
                await _context.SaveChangesAsync();
                //System.Diagnostics.Debug.WriteLine("Creating car - p6");
                return RedirectToAction(nameof(Index));
            //}
            //System.Diagnostics.Debug.WriteLine("Creating car - p7 - notValid");
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            var customerTable = _context.Customers
            .Select(s => new
            {
                Id = s.Id,
                CustomerDescription = string.Format("{0} {1}", s.Name, s.LastName)
            }).ToList();
            ViewData["CustomerId"] = new SelectList(customerTable, "Id", "CustomerDescription");
            //ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", car.CustomerId);
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,Name,Description,Brand,Model,VIN,RegistrationPlate,ManufacturedYear,imageUrl")] Car car)
        {
            if (id != car.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                    await HandleUploadAsync(car);

                _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            //}
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", car.CustomerId);
            return View(car);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }

        private async Task HandleUploadAsync(Car target)
        {
            var upload = Request.Form.Files.FirstOrDefault();
            System.Diagnostics.Debug.WriteLine("Uploading image - i1");
            if (upload == null || upload.Length == 0) return;
            System.Diagnostics.Debug.WriteLine("Uploading image - i2");
            var ext = Path.GetExtension(upload.FileName).ToLowerInvariant();
            System.Diagnostics.Debug.WriteLine("Uploading image - i3");
            if (ext is not ".png" and not ".jpg" and not ".jpeg")
            {
                System.Diagnostics.Debug.WriteLine("Uploading image - i4 - bad extension!");
                ModelState.AddModelError("upload", "Tylko PNG lub JPG");
                throw new InvalidOperationException("Wrong file type");
            }

            var fileName = $"{Guid.NewGuid()}{ext}";
            System.Diagnostics.Debug.WriteLine("Uploading image - i5:"+fileName);
            var savePath = Path.Combine(_env.WebRootPath, "uploads", fileName);
            await using var fs = new FileStream(savePath, FileMode.Create);
            System.Diagnostics.Debug.WriteLine("Uploading image - i6");
            await upload.CopyToAsync(fs);
            target.imageUrl = $"/uploads/{fileName}";
            System.Diagnostics.Debug.WriteLine("Image url"+target.imageUrl);
        }

    }
}
