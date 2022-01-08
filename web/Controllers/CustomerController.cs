using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;
using Microsoft.AspNetCore.Authorization;

namespace web.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly WarehouseContext _context;

        public CustomerController(WarehouseContext context)
        {
            _context = context;
        }

        // GET: Customer
        public async Task<IActionResult> Index(
        string sortOrder,
        string currentFilter,
        string searchString,
        int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["FirstNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "firstname_desc" : "";
            ViewData["LastNameSortParm"] = sortOrder == "lastname" ? "lastname_desc" : "lastname";
            ViewData["AddressSortParm"] = sortOrder == "address" ? "address_desc" : "address";
            ViewData["ZIPSortParm"] = sortOrder == "zip" ? "zip_desc" : "zip";
            ViewData["CitySortParm"] = sortOrder == "city" ? "city_desc" : "city";
            ViewData["CountrySortParm"] = sortOrder == "country" ? "country_desc" : "country";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            
            ViewData["CurrentFilter"] = searchString;

            var customers = from c in _context.Customers
                            select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                Int64 anumber = 9999999;
                if (Int64.TryParse(searchString,out anumber)) {}
                customers = customers.Where(i => i.FirstName.Contains(searchString)
                                || i.LastName.Contains(searchString)
                                || i.Address.Contains(searchString)
                                || (i.ZIP == anumber && anumber != 9999999)
                                || i.City.Contains(searchString)
                                || i.Country.Contains(searchString)
                                );
            }

            switch (sortOrder)
            {
                case "firstname_desc":
                    customers = customers.OrderByDescending(i => i.FirstName);
                    break;
                case "lastname":
                    customers = customers.OrderBy(i => i.LastName);
                    break;
                case "lastname_desc":
                    customers = customers.OrderByDescending(i => i.LastName);
                    break;
                case "address":
                    customers = customers.OrderBy(i => i.Address);
                    break;
                case "address_desc":
                    customers = customers.OrderByDescending(i => i.Address);
                    break;
                case "zip":
                    customers = customers.OrderBy(i => i.ZIP);
                    break;
                case "zip_desc":
                    customers = customers.OrderByDescending(i => i.ZIP);
                    break;
                case "city":
                    customers = customers.OrderBy(i => i.City);
                    break;
                case "city_desc":
                    customers = customers.OrderByDescending(i => i.City);
                    break;
                case "country":
                    customers = customers.OrderBy(i => i.Country);
                    break;
                case "country_desc":
                    customers = customers.OrderByDescending(i => i.Country);
                    break;
                default:
                    customers = customers.OrderBy(i => i.FirstName);
                    break;
            }


            int pageSize = 6;
            return View(await PaginatedList<Customer>.CreateAsync(customers.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customer/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,Address,ZIP,City,Country")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customer/Edit/5
        [Authorize(Roles = "Administrator, Manager")]
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

        // POST: Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,Address,ZIP,City,Country")] Customer customer)
        {
            if (id != customer.ID)
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
                    if (!CustomerExists(customer.ID))
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

        // GET: Customer/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.ID == id);
        }
    }
}
