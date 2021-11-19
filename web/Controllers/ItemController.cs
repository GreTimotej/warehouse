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
    public class ItemController : Controller
    {
        private readonly WarehouseContext _context;

        public ItemController(WarehouseContext context)
        {
            _context = context;
        }

        // GET: Item
        public async Task<IActionResult> Index(string sortOrder)
        {

            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            var warehouseContext = _context.Items.Include(i => i.Customer).Include(i => i.Warehouse);
            var items = from i in warehouseContext
                        select i;

            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(i => i.Name);
                    break;
                case "desc":
                    items = items.OrderBy(i => i.Description);
                    break;
                case "desc_desc":
                    items = items.OrderByDescending(i => i.Description);
                    break;
                case "customer":
                    items = items.OrderBy(i => i.Customer);
                    break;
                case "customer_desc":
                    items = items.OrderByDescending(i => i.Customer);
                    break;
                case "warehouse":
                    items = items.OrderBy(i => i.Warehouse);
                    break;
                case "warehouse_desc":
                    items = items.OrderByDescending(i => i.Warehouse);
                    break;
                default:
                    items = items.OrderBy(i => i.Name);
                    break;
            }

            

            return View(await items.AsNoTracking().ToListAsync());
        }

        // GET: Item/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Customer)
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Item/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "ID");
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "ID", "ID");
            return View();
        }

        // POST: Item/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,Quantity,WarehouseID,CustomerID")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "ID", item.CustomerID);
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "ID", "ID", item.WarehouseID);
            return View(item);
        }

        // GET: Item/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "ID", item.CustomerID);
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "ID", "ID", item.WarehouseID);
            return View(item);
        }

        // POST: Item/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,Quantity,WarehouseID,CustomerID")] Item item)
        {
            if (id != item.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ID))
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
            ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "ID", item.CustomerID);
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "ID", "ID", item.WarehouseID);
            return View(item);
        }

        // GET: Item/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Customer)
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ID == id);
        }
    }
}
