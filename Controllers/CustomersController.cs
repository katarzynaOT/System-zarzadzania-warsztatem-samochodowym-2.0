using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkshopManager.Data;
using WorkshopManager.Models;

namespace WorkshopManager.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        /*
         public async Task<IActionResult> Index()
         {
             return View(await _context.Customers.ToListAsync());
         }
        */

        public async Task<IActionResult> Index(int? id, int? carId)
        {
            var viewModel = new CustomerIndexData();
            viewModel.Customers = await _context.Customers
                .Include(i => i.Cars)
                .AsNoTracking()
                .OrderBy(i => i.Name)
                .ToListAsync();

            if (id != null)
            {
                ViewData["CustomerID"] = id.Value;
                Customer customer = viewModel.Customers.Where(
                    i => i.Id == id.Value).Single();
                viewModel.Cars = customer.Cars;
            }
            System.Diagnostics.Debug.WriteLine("Selected carId:"+ carId);

            if (carId != null)
            {
                ViewData["CarID"] = carId.Value;
                Customer customer = viewModel.Customers.Where(
                  i => i.Id == id.Value).Single();
                Car car = customer.Cars.Where(
                    id =>id.Id == carId.Value).Single();
                viewModel.selectedCar = car;

                viewModel.SelectedCarOrders = car.Orders;

                viewModel.SelectedCarOrders = await _context.ServiceOrders.Where(i => i.CarId == car.Id).ToListAsync();
                System.Diagnostics.Debug.WriteLine("Car orders for:" + car.Name + " are:"+viewModel.SelectedCarOrders.ToString);
                foreach (var item in viewModel.SelectedCarOrders)
                {
                    System.Diagnostics.Debug.WriteLine("Diagnostics for car:" + car.Name+ " "+item.Id + " done by "+item.AssignedMechanic);
                }

                System.Diagnostics.Debug.WriteLine("Found car:"+car.Name);

            }
            return View(viewModel);
        }

        public async Task<IActionResult> Report(int? id, int? carId, int? customerid)
        {
            var viewModel = new CustomerIndexData();
            viewModel.Customers = await _context.Customers
                .Include(i => i.Cars)
                .AsNoTracking()
                .OrderBy(i => i.Name)
                .ToListAsync();

            if (customerid!=null)
            {
                viewModel.selectedCustomer = await _context.Customers.Where(i=> i.Id == customerid).SingleAsync();

                //reports of all customer orders/across all cars
                List<ServiceOrder> allOrders = new List<ServiceOrder>();
                List<Car> customerCars = await _context.Cars.Where(i => i.CustomerId == customerid).ToListAsync();
                foreach (var car in customerCars) {
                    List<ServiceOrder> orders = await _context.ServiceOrders.Where(i => i.CarId == car.Id).ToListAsync();
                    //viewModel.SelectedCarOrders = await _context.ServiceOrders.Where(i => i.CarId == car.Id).ToListAsync();
                    foreach (var order in orders)
                    {
                        order.Car = car; //add car to get the name of it in the list nicely
                    }
                    allOrders.AddRange(orders);
                }
                viewModel.SelectedCarOrders= allOrders;

                return View(viewModel);
            }


            if (id != null)
            {
                ViewData["CustomerID"] = id.Value;
                Customer customer = viewModel.Customers.Where(
                    i => i.Id == id.Value).Single();
                viewModel.Cars = customer.Cars;
            }
            System.Diagnostics.Debug.WriteLine("Selected carId:" + carId);

            if (carId != null)
            {
                ViewData["CarID"] = carId.Value;
                Customer customer = viewModel.Customers.Where(
                  i => i.Id == id.Value).Single();
                Car car = customer.Cars.Where(
                    id => id.Id == carId.Value).Single();
                viewModel.selectedCar = car;
                viewModel.selectedCustomer = customer;

                viewModel.SelectedCarOrders = car.Orders;

                viewModel.SelectedCarOrders = await _context.ServiceOrders.Where(i => i.CarId == car.Id).ToListAsync();
                System.Diagnostics.Debug.WriteLine("Car orders for:" + car.Name + " are:" + viewModel.SelectedCarOrders.ToString);
                foreach (var item in viewModel.SelectedCarOrders)
                {
                    //System.Diagnostics.Debug.WriteLine("Diagnostics for car:" + car.Name + " " + item.Id + " done by " + item.AssignedMechanic);
                    item.Car = car;
                }

                System.Diagnostics.Debug.WriteLine("Found car:" + car.Name);

            }
            return View(viewModel);
        }
        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,LastName,Email,Phone,Address")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }



        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,LastName,Email,Phone,Address")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
