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
    public class DistributorController : Controller
    {
        private readonly WarehouseContext _context;

        public DistributorController(WarehouseContext context)
        {
            _context = context;
        }

        // GET: Distributor
        public async Task<IActionResult> Index(
        string sortOrder,
        string currentFilter,
        string searchString,
        int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
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

            var distributors = from d in _context.Distributors
                            select d;

            if (!String.IsNullOrEmpty(searchString))
            {
                Int64 anumber = 9999999;
                if (Int64.TryParse(searchString,out anumber)) {}
                distributors = distributors.Where(i => i.Name.Contains(searchString)
                                || i.Address.Contains(searchString)
                                || (i.ZIP == anumber && anumber != 9999999)
                                || i.City.Contains(searchString)
                                || i.Country.Contains(searchString)
                                );
            }

            switch (sortOrder)
            {
                case "name_desc":
                    distributors = distributors.OrderByDescending(i => i.Name);
                    break;
                case "address":
                    distributors = distributors.OrderBy(i => i.Address);
                    break;
                case "address_desc":
                    distributors = distributors.OrderByDescending(i => i.Address);
                    break;
                case "zip":
                    distributors = distributors.OrderBy(i => i.ZIP);
                    break;
                case "zip_desc":
                    distributors = distributors.OrderByDescending(i => i.ZIP);
                    break;
                case "city":
                    distributors = distributors.OrderBy(i => i.City);
                    break;
                case "city_desc":
                    distributors = distributors.OrderByDescending(i => i.City);
                    break;
                case "country":
                    distributors = distributors.OrderBy(i => i.Country);
                    break;
                case "country_desc":
                    distributors = distributors.OrderByDescending(i => i.Country);
                    break;
                default:
                    distributors = distributors.OrderBy(i => i.Name);
                    break;
            }

            int pageSize = 6;
            return View(await PaginatedList<Distributor>.CreateAsync(distributors.AsNoTracking(), pageNumber ?? 1, pageSize));
        
        }

        // GET: Distributor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distributor = await _context.Distributors
                .FirstOrDefaultAsync(m => m.ID == id);
            if (distributor == null)
            {
                return NotFound();
            }

            return View(distributor);
        }

        // GET: Distributor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Distributor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Address,ZIP,City,Country")] Distributor distributor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(distributor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(distributor);
        }

        // GET: Distributor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distributor = await _context.Distributors.FindAsync(id);
            if (distributor == null)
            {
                return NotFound();
            }
            return View(distributor);
        }

        // POST: Distributor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Address,ZIP,City,Country")] Distributor distributor)
        {
            if (id != distributor.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(distributor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DistributorExists(distributor.ID))
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
            return View(distributor);
        }

        // GET: Distributor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distributor = await _context.Distributors
                .FirstOrDefaultAsync(m => m.ID == id);
            if (distributor == null)
            {
                return NotFound();
            }

            return View(distributor);
        }

        // POST: Distributor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var distributor = await _context.Distributors.FindAsync(id);
            _context.Distributors.Remove(distributor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DistributorExists(int id)
        {
            return _context.Distributors.Any(e => e.ID == id);
        }
    }
}
