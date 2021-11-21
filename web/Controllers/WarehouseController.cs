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
    [Authorize(Roles = "Administrator, Manager, Staff")]
    public class WarehouseController : Controller
    {
        private readonly WarehouseContext _context;

        public WarehouseController(WarehouseContext context)
        {
            _context = context;
        }

        // GET: Warehouse
        
        public async Task<IActionResult> Index(
        string sortOrder,
        string currentFilter,
        string searchString,
        int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["AddressSortParm"] = String.IsNullOrEmpty(sortOrder) ? "address_desc" : "";
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

            var warehouses = from w in _context.Warehouses
                            select w;

            if (!String.IsNullOrEmpty(searchString))
            {
                Int64 anumber = 9999999;
                if (Int64.TryParse(searchString,out anumber)) {}
                warehouses = warehouses.Where(i => i.Address.Contains(searchString)
                                || (i.ZIP == anumber && anumber != 9999999)
                                || i.City.Contains(searchString)
                                || i.Country.Contains(searchString)
                                );
            }

            switch (sortOrder)
            {
                case "address_desc":
                    warehouses = warehouses.OrderByDescending(i => i.Address);
                    break;
                case "zip":
                    warehouses = warehouses.OrderBy(i => i.ZIP);
                    break;
                case "zip_desc":
                    warehouses = warehouses.OrderByDescending(i => i.ZIP);
                    break;
                case "city":
                    warehouses = warehouses.OrderBy(i => i.City);
                    break;
                case "city_desc":
                    warehouses = warehouses.OrderByDescending(i => i.City);
                    break;
                case "country":
                    warehouses = warehouses.OrderBy(i => i.Country);
                    break;
                case "country_desc":
                    warehouses = warehouses.OrderByDescending(i => i.Country);
                    break;
                default:
                    warehouses = warehouses.OrderBy(i => i.Address);
                    break;
            }

            int pageSize = 6;
            return View(await PaginatedList<Warehouse>.CreateAsync(warehouses.AsNoTracking(), pageNumber ?? 1, pageSize));
        
        }

        // GET: Warehouse/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouses
                .FirstOrDefaultAsync(m => m.ID == id);
            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }

        [Authorize(Roles = "Administartor")]
        // GET: Warehouse/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Warehouse/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Address,ZIP,City,Country")] Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(warehouse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(warehouse);
        }

        [Authorize(Roles = "Administrator, Manager")]
        // GET: Warehouse/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null)
            {
                return NotFound();
            }
            return View(warehouse);
        }

        // POST: Warehouse/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Address,ZIP,City,Country")] Warehouse warehouse)
        {
            if (id != warehouse.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(warehouse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WarehouseExists(warehouse.ID))
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
            return View(warehouse);
        }

        [Authorize(Roles = "Administartor")]
        // GET: Warehouse/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouses
                .FirstOrDefaultAsync(m => m.ID == id);
            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }

        // POST: Warehouse/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            _context.Warehouses.Remove(warehouse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WarehouseExists(int id)
        {
            return _context.Warehouses.Any(e => e.ID == id);
        }
    }
}
