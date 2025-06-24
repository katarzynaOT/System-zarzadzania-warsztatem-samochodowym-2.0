using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using WorkshopManager.Data;
using WorkshopManager.Documents;
using WorkshopManager.Models;
using WorkshopManager.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;

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
            viewModel.Comments = comments;
            viewModel.ServiceTasks = await _context.ServiceTasks.Where(i => i.OrderId == serviceOrder.Id).ToListAsync();

            foreach (var serviceTask in viewModel.ServiceTasks)
            {
                List<UsedPart> globalParts = new List<UsedPart>();
                List<UsedPart> parts = await _context.UsedParts.Where(i => i.ServiceTaskId == serviceTask.Id).ToListAsync();
                globalParts.AddRange(parts);
                viewModel.UsedParts = globalParts;
            }

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
            ViewData["MechanikId"] = await getMechanicUsersHelper();
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
                if (rolesForUser[0].Contains("Mechanik"))
                {
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
        public async Task<IActionResult> Edit(int? id, int? recalc)
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

            if (recalc != null && recalc > 0)
            {
                //calculate whole order costs
                //cost of all tasks
                //cost of all parts for a given task
                //await _context.ServiceOrders.Where(i => i.CarId == car.Id).ToListAsync();
                int totalCost = 0;
                List<ServiceTask> serviceTasks = await _context.ServiceTasks.Where(i => i.OrderId == serviceOrder.Id).ToListAsync();
                foreach (var task in serviceTasks)
                {
                    System.Diagnostics.Debug.WriteLine("Labor cost for task " + task.Title + " is:" + task.LaborCost);
                    totalCost += task.LaborCost;
                    List<UsedPart> usedParts = await _context.UsedParts.Where(i => i.ServiceTaskId == task.Id).ToListAsync();
                    foreach (var part in usedParts)
                    {
                        System.Diagnostics.Debug.WriteLine("\tParts cost for task " + task.Title + " is:" + part.TotalCost);
                        totalCost += part.TotalCost;
                    }
                }
                System.Diagnostics.Debug.WriteLine("Total labor cost:" + totalCost);
                serviceOrder.Price = totalCost;
            }
            System.Diagnostics.Debug.WriteLine("Passing down to total labor cost:" + serviceOrder.Price);
            return View(serviceOrder);
        }

        // POST: ServiceOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CarId,Description,Status,CompletedDate,AssignedMechanic,Price")] ServiceOrder serviceOrder)
        {
            if (id != serviceOrder.Id)
            {
                return NotFound();
            }

            System.Diagnostics.Debug.WriteLine("Saving ServiceOrder with total cost:" + serviceOrder.Price);

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


        //public async Task<IActionResult> Archive(string? FromDate, string? ToDate, string? SearchName, string? SearchCar)
        //{
        //    ServiceArchive archive = new ServiceArchive();
        //    System.Diagnostics.Debug.WriteLine("Archive for orders invoked");

        //    if (SearchName != null && SearchCar != null)
        //    {
        //        System.Diagnostics.Debug.WriteLine("Archive for orders:" + FromDate + " " + ToDate + " " + SearchName + " " + SearchCar);
        //        archive.SearchCar = SearchCar;
        //        archive.SearchName = SearchName;
        //        DateTime from;
        //        DateTime.TryParse(FromDate, out from);
        //        DateTime to;
        //        DateTime.TryParse(ToDate, out to);

        //        archive.FromDate = from;
        //        archive.ToDate = to;

        //        List<ServiceRecord> filteredOrders = new List<ServiceRecord>();
        //        List<ServiceOrder> orders = await _context.ServiceOrders.ToListAsync();
        //        foreach (var order in orders)
        //        {
        //            System.Diagnostics.Debug.WriteLine("Checking order:" + order.Id);

        //            String orderDateString = order.CompletedDate;
        //            DateTime orderDateTime;
        //            System.Diagnostics.Debug.WriteLine("Order completed date" + orderDateString);

        //            Car car = await _context.Cars.Where(i => i.Id == order.CarId).FirstAsync();
        //            Customer customer = await _context.Customers.Where(i => i.Id == car.CustomerId).FirstAsync();

        //            System.Diagnostics.Debug.WriteLine("Customer Name" + customer.Name + "Car name " + car.Name);
        //            if (customer.Name.Contains(SearchName) || car.Name.Contains(SearchCar))
        //                if (DateTime.TryParse(orderDateString, out orderDateTime))
        //                {
        //                    System.Diagnostics.Debug.WriteLine("Order completed date" + orderDateString);
        //                    System.Diagnostics.Debug.WriteLine("Date order" + orderDateTime);
        //                    System.Diagnostics.Debug.WriteLine("Date From" + from);
        //                    System.Diagnostics.Debug.WriteLine("Date to" + to);
        //                    if (orderDateTime > from && orderDateTime < to)
        //                    {
        //                        ServiceRecord record = new ServiceRecord();
        //                        record.CarName = car.Name;
        //                        record.CustomerName = customer.Name;
        //                        record.Id = order.Id;
        //                        record.TotalCost = order.Price;
        //                        record.Description = order.Description;
        //                        if (DateTime.TryParse(orderDateString, out orderDateTime))
        //                            record.CompletedDate = orderDateTime;
        //                        filteredOrders.Add(record);
        //                        System.Diagnostics.Debug.WriteLine("Appended order to result list:" + order.Id);
        //                    }
        //                }
        //        }
        //        foreach (var record in filteredOrders)
        //        {
        //            System.Diagnostics.Debug.WriteLine("Order:" + record.Id + " " + record.Description + " " + record.CarName);
        //        }
        //        archive.SearchResults = filteredOrders;
        //    }
        //    return View(archive);
        //}
        public async Task<IActionResult> Archive(string? FromDate, string? ToDate, string? SearchName, string? SearchCar)
        {
            ServiceArchive archive = new ServiceArchive();
            System.Diagnostics.Debug.WriteLine("Archive for orders invoked");
            DateTime from;
            DateTime.TryParse(FromDate, out from);
            DateTime to;
            DateTime.TryParse(ToDate, out to);

            archive.FromDate = from;
            archive.ToDate = to;

            List<ServiceRecord> filteredOrders = new List<ServiceRecord>();
            List<ServiceOrder> orders = await _context.ServiceOrders.ToListAsync();
            foreach (var order in orders)
            {
                System.Diagnostics.Debug.WriteLine("Checking order:" + order.Id);

                String orderDateString = order.CompletedDate;
                DateTime orderDateTime;
                System.Diagnostics.Debug.WriteLine("Order completed date" + orderDateString);

                Car car = await _context.Cars.Where(i => i.Id == order.CarId).FirstAsync();
                Customer customer = await _context.Customers.Where(i => i.Id == car.CustomerId).FirstAsync();

                System.Diagnostics.Debug.WriteLine("Customer Name" + customer.Name + "Car name " + car.Name);
                //if (customer.Name.Contains(SearchName) || car.Name.Contains(SearchCar))
                if (DateTime.TryParse(orderDateString, out orderDateTime))
                {
                    System.Diagnostics.Debug.WriteLine("Order completed date" + orderDateString);
                    System.Diagnostics.Debug.WriteLine("Date order" + orderDateTime);
                    System.Diagnostics.Debug.WriteLine("Date From" + from);
                    System.Diagnostics.Debug.WriteLine("Date to" + to);
                    if (orderDateTime > from && orderDateTime < to)
                    {
                        ServiceRecord record = new ServiceRecord();
                        record.CarName = car.Name;
                        record.CustomerName = customer.Name;
                        record.Id = order.Id;
                        record.TotalCost = order.Price;
                        record.Description = order.Description;
                        if (DateTime.TryParse(orderDateString, out orderDateTime))
                            record.CompletedDate = orderDateTime;
                        filteredOrders.Add(record);
                        System.Diagnostics.Debug.WriteLine("Appended order to result list:" + order.Id);
                    }
                }
            }
            foreach (var record in filteredOrders)
            {
                System.Diagnostics.Debug.WriteLine("Order:" + record.Id + " " + record.Description + " " + record.CarName);
            }
            if (filteredOrders != null)
            {
                filteredOrders.Sort((x, y) => DateTime.Compare(x.CompletedDate, y.CompletedDate));
            }
            archive.SearchResults = filteredOrders;
            return View(archive);
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

        [HttpPost]
        public async Task<IActionResult> GenerateReportPdf(ServiceArchive model)
        {
            var query = _context.ServiceOrders
                .Include(o => o.Car)
                .ThenInclude(c => c.Customer)
                .AsQueryable();

            if (!string.IsNullOrEmpty(model.SearchName))
                query = query.Where(o => o.Car.Customer.Name.Contains(model.SearchName));

            if (!string.IsNullOrEmpty(model.SearchCar))
                query = query.Where(o => o.Car.Name.Contains(model.SearchCar));

            var results = await query.ToListAsync();

            if (model.FromDate.HasValue)
            {
                results = results.Where(o =>
                    DateTime.TryParse(o.CompletedDate, out var parsed) &&
                    parsed >= model.FromDate.Value).ToList();
            }

            if (model.ToDate.HasValue)
            {
                results = results.Where(o =>
                    DateTime.TryParse(o.CompletedDate, out var parsed) &&
                    parsed <= model.ToDate.Value).ToList();
            }

            var document = new ServiceOrdersReportDocument(results);
            var pdf = document.GeneratePdf();

            return File(pdf, "application/pdf", "raport.pdf");
        }


    }
}
