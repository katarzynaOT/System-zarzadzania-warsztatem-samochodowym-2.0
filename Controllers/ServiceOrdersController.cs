using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using WorkshopManager.Data;
using WorkshopManager.Models;
using WorkshopManager.Services;

namespace WorkshopManager.Controllers
{
    public class ServiceOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public ServiceOrdersController(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // GET: ServiceOrders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ServiceOrders.Include(s => s.Car);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ServiceOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceOrder = await _context.ServiceOrders
                .Include(s => s.Car)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceOrder == null)
            {
                return NotFound();
            }

            var viewModel = new OrderIndexData();
            viewModel.Order = serviceOrder;
            //viewModel.Comments = await _context.Comments.Where(i => i.OrderId == serviceOrder.Id).ToListAsync();
            List<Comment> comments = await _context.Comments.Where(i => i.OrderId == serviceOrder.Id).ToListAsync();
            comments.Sort((x, y) => DateTime.Compare(x.CreatedDate, y.CreatedDate));
            viewModel.Comments= comments;
            viewModel.ServiceTasks = await _context.ServiceTasks.Where(i => i.OrderId == serviceOrder.Id).ToListAsync();

            //return View(serviceOrder);
            return View(viewModel);
        }

        // GET: ServiceOrders/Create
        public async Task<IActionResult> Create(int? car_id)
        {
            if (car_id != null)
            {
                var carsTable = _context.Cars
                .Where(s => s.Id == car_id)
                .Select(s => new
                    {
                    Id = s.Id,
                    CarDescription = string.Format("{0} {1}", s.Name, s.Brand)
                    }).ToList();
                ViewData["CarId"] = new SelectList(carsTable, "Id", "CarDescription");
            }
            else
            {
                var carsTable = _context.Cars
                 .Select(s => new
                 {
                     Id = s.Id,
                     CarDescription = string.Format("{0} {1}", s.Name, s.Brand)
                 }).ToList();
                ViewData["CarId"] = new SelectList(carsTable, "Id", "CarDescription");
            }
            ViewData["StatusId"] = getStatusListHelper();
            ViewData["MechanikId"]= await getMechanicUsersHelper();
            return View();
        }

        public async Task<List<SelectListItem>> getMechanicUsersHelper()
        {
            var users = await _userService.GetAllUsersAsync();
            var roles = await _userService.GetUserRolesAsync(users);
            List<SelectListItem> mechList = new List<SelectListItem>();
            foreach (var user in users)
            {
                var rolesForUser = roles[user.Id];
                if (rolesForUser[0].Contains("Mechanik")) {
                    mechList.Add(new SelectListItem(user.FirstName + user.LastName, user.FirstName + user.LastName));
                }
            }
            return mechList;
        }

        List<SelectListItem> getStatusListHelper()
        {
            List<SelectListItem> statusList = new List<SelectListItem>() {
                new SelectListItem {
                    Text = "New", Value = "New"
                     },
                new SelectListItem {
                    Text = "Assigned", Value = "Assigned"
                    },
                new SelectListItem {
                    Text = "In progress", Value = "In progress"
                     },
                new SelectListItem {
                    Text = "Completed", Value = "Completed"
                     },
                new SelectListItem {
                    Text = "Paid", Value = "Paid"
                     },
                new SelectListItem {
                    Text = "Reopened", Value = "Reopened"
                     },
            };
            return statusList;
        }
        // POST: ServiceOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CarId,Description,Status,CompletedDate,AssignedMechanic,Price")] ServiceOrder serviceOrder)
        {
            //if (ModelState.IsValid)
            //{
                _context.Add(serviceOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id", serviceOrder.CarId);
            return View(serviceOrder);
        }

        // GET: ServiceOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceOrder = await _context.ServiceOrders.FindAsync(id);
            if (serviceOrder == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id", serviceOrder.CarId);

            ViewData["StatusId"] = getStatusListHelper();
            ViewData["MechanikId"] = await getMechanicUsersHelper();
            return View(serviceOrder);
        }

        // POST: ServiceOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CarId,Description,Status,CompletedDate, AssignedMechanic,Price")] ServiceOrder serviceOrder)
        {
            if (id != serviceOrder.Id)
            {
                return NotFound();
            }

            if (serviceOrder.Status.Contains("Completed"))
            {
                serviceOrder.CompletedDate = DateTime.Now.ToString();
            }
            else
            {
                serviceOrder.CompletedDate = "Order not yet complete";
            }
                //if (ModelState.IsValid)
                //{
                try
                {
                    _context.Update(serviceOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceOrderExists(serviceOrder.Id))
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
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id", serviceOrder.CarId);
            return View(serviceOrder);
        }

        // GET: ServiceOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceOrder = await _context.ServiceOrders
                .Include(s => s.Car)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceOrder == null)
            {
                return NotFound();
            }

            return View(serviceOrder);
        }

        // POST: ServiceOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceOrder = await _context.ServiceOrders.FindAsync(id);
            if (serviceOrder != null)
            {
                _context.ServiceOrders.Remove(serviceOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceOrderExists(int id)
        {
            return _context.ServiceOrders.Any(e => e.Id == id);
        }
    }
}
