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

namespace WorkshopManager.Controllers
{
    public class ServiceTasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceTasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ServiceTasks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ServiceTasks.Include(s => s.Order);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ServiceTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceTask = await _context.ServiceTasks
                .Include(s => s.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceTask == null)
            {
                return NotFound();
            }

            var viewModel = new TaskIndexData();
            viewModel.Task = serviceTask;
            viewModel.TaskParts = await _context.UsedParts.Where(i => i.ServiceTaskId == serviceTask.Id).ToListAsync();

            foreach (var taskPart in viewModel.TaskParts)
            {
                taskPart.Part = await _context.Parts.Where(i => i.Id == taskPart.PartId).SingleAsync();
                //System.Diagnostics.Debug.WriteLine("Found part:" + taskPart.Part.Name);
            }

            return View(viewModel);
        }

        // GET: ServiceTasks/Create
        public IActionResult Create(int? serviceorder_id)
        {
            if (serviceorder_id != null)
            {
                var ordersTable = _context.ServiceOrders
                .Where(s => s.Id == serviceorder_id)
                .Select(s => new
                {
                    Id = s.Id
                }).ToList();
                ViewData["OrderId"] = new SelectList(ordersTable, "Id", "Id");
                return View();
            }
            else
            {
                ViewData["OrderId"] = new SelectList(_context.ServiceOrders, "Id", "Id");
                return View();
            }
        }

        // POST: ServiceTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderId,Description,Title,LaborCost")] ServiceTask serviceTask)
        {
            //if (ModelState.IsValid)
            //{
            _context.Add(serviceTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            //}
            ViewData["OrderId"] = new SelectList(_context.ServiceOrders, "Id", "Id", serviceTask.OrderId);
            return View(serviceTask);
        }

        // GET: ServiceTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceTask = await _context.ServiceTasks.FindAsync(id);
            if (serviceTask == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.ServiceOrders, "Id", "Id", serviceTask.OrderId);
            return View(serviceTask);
        }

        // POST: ServiceTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderId,Description,Title,LaborCost")] ServiceTask serviceTask)
        {
            if (id != serviceTask.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
            try
            {
                _context.Update(serviceTask);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceTaskExists(serviceTask.Id))
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
            ViewData["OrderId"] = new SelectList(_context.ServiceOrders, "Id", "Id", serviceTask.OrderId);
            return View(serviceTask);
        }

        // GET: ServiceTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceTask = await _context.ServiceTasks
                .Include(s => s.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceTask == null)
            {
                return NotFound();
            }

            return View(serviceTask);
        }

        // POST: ServiceTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceTask = await _context.ServiceTasks.FindAsync(id);
            if (serviceTask != null)
            {
                _context.ServiceTasks.Remove(serviceTask);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceTaskExists(int id)
        {
            return _context.ServiceTasks.Any(e => e.Id == id);
        }
    }
}
