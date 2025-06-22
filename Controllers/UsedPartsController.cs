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
    public class UsedPartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsedPartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UsedParts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UsedParts.Include(u => u.Part).Include(u => u.ServiceTask);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UsedParts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usedPart = await _context.UsedParts
                .Include(u => u.Part)
                .Include(u => u.ServiceTask)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usedPart == null)
            {
                return NotFound();
            }

            return View(usedPart);
        }

        // GET: UsedParts/Create
        public IActionResult Create(int? servicetask_id, int? recalc, int? quantity, int? partId)
        {
            var partsTable = _context.Parts
            .Select(s => new
                {
                Id = s.Id,
                PartName = string.Format("{0} - {1}", s.Type, s.Name)
             }).ToList();
            ViewData["PartId"] = new SelectList(partsTable, "Id", "PartName");

            if (servicetask_id != null)
            {
                ViewData["ServiceTaskId"] = new List<SelectListItem>() {
                new SelectListItem {
                    Text = servicetask_id.ToString(), Value = servicetask_id.ToString()
                     }
                };
            }
            else
            {
                //ViewData["PartId"] = new SelectList(_context.Parts, "Id", "Id");
                ViewData["ServiceTaskId"] = new SelectList(_context.ServiceTasks, "Id", "Id");
            }

            if (recalc != null && recalc > 0 && quantity != null)
            {
                System.Diagnostics.Debug.WriteLine("Used Parts recalc request with quantity:" + quantity);

                var part = _context.Parts.Find(partId);
                if (part != null)
                {
                    var partsCost = part.UnitPrice * (int)(quantity ?? 1);
                    ViewData["NewCost"] = partsCost;

                }
                System.Diagnostics.Debug.WriteLine("ViewData[NewCost]:" + ViewData["NewCost"]);
            }
            else ViewData["NewCost"] = 0;

                return View();
        }

        // POST: UsedParts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PartId,ServiceTaskId,Quantity,TotalCost")] UsedPart usedPart)
        {
            //if (ModelState.IsValid)
            //{
                _context.Add(usedPart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            ViewData["PartId"] = new SelectList(_context.Parts, "Id", "Id", usedPart.PartId);
            ViewData["ServiceTaskId"] = new SelectList(_context.ServiceTasks, "Id", "Id", usedPart.ServiceTaskId);
            return View(usedPart);
        }

        // GET: UsedParts/Edit/5
        public async Task<IActionResult> Edit(int? id, int? servicetask_id, int? recalc, int? quantity, int? partId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usedPart = await _context.UsedParts.FindAsync(id);
            if (usedPart == null)
            {
                return NotFound();
            }

            if (recalc != null && recalc > 0 && quantity !=null)
            {
                System.Diagnostics.Debug.WriteLine("Used Parts recalc request with quantity:"+quantity);
                
                var part = await _context.Parts.FindAsync(partId);
                if (part != null)
                {
                    var partsCost = part.UnitPrice * (int)(quantity ?? 1);
                    usedPart.TotalCost = partsCost;
                }
            }

            var partsTable = _context.Parts
            .Select(s => new
               {
                Id = s.Id,
                PartName = string.Format("{0} - {1}", s.Type, s.Name)
                }).ToList();
            ViewData["PartId"] = new SelectList(partsTable, "Id", "PartName");

            if (servicetask_id != null)
            {
                ViewData["ServiceTaskId"] = new List<SelectListItem>() {
                new SelectListItem {
                    Text = servicetask_id.ToString(), Value = servicetask_id.ToString()
                     }
                };
            }
            else
            {
                //ViewData["PartId"] = new SelectList(_context.Parts, "Id", "Id");
                ViewData["ServiceTaskId"] = new SelectList(_context.ServiceTasks, "Id", "Id");
            }

            //ViewData["PartId"] = new SelectList(_context.Parts, "Id", "Id", usedPart.PartId);
            //ViewData["ServiceTaskId"] = new SelectList(_context.ServiceTasks, "Id", "Id", usedPart.ServiceTaskId);
            return View(usedPart);
        }

        // POST: UsedParts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PartId,ServiceTaskId,Quantity,TotalCost")] UsedPart usedPart, int? recalc)
        {
            if (id != usedPart.Id)
            {
                return NotFound();
            }


            //if (ModelState.IsValid)
            //{
            try
            {
                    _context.Update(usedPart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsedPartExists(usedPart.Id))
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
            ViewData["PartId"] = new SelectList(_context.Parts, "Id", "Id", usedPart.PartId);
            ViewData["ServiceTaskId"] = new SelectList(_context.ServiceTasks, "Id", "Id", usedPart.ServiceTaskId);
            return View(usedPart);
        }

        // GET: UsedParts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usedPart = await _context.UsedParts
                .Include(u => u.Part)
                .Include(u => u.ServiceTask)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usedPart == null)
            {
                return NotFound();
            }

            return View(usedPart);
        }

        // POST: UsedParts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usedPart = await _context.UsedParts.FindAsync(id);
            if (usedPart != null)
            {
                _context.UsedParts.Remove(usedPart);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsedPartExists(int id)
        {
            return _context.UsedParts.Any(e => e.Id == id);
        }
    }
}
