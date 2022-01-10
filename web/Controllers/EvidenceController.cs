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
    public class EvidenceController : Controller
    {
        private readonly WarehouseContext _context;

        public EvidenceController(WarehouseContext context)
        {
            _context = context;
        }

        // GET: Evidence
        public async Task<IActionResult> Index(
        string sortOrder,
        string currentFilter,
        string searchString,
        int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["OutSortParm"] = String.IsNullOrEmpty(sortOrder) ? "out_desc" : "";
            ViewData["WarehouseSortParm"] = sortOrder == "warehouse" ? "warehouse_desc" : "warehouse";
            ViewData["ItemSortParm"] = sortOrder == "item" ? "item_desc" : "item";
            ViewData["CustomerSortParm"] = sortOrder == "customer" ? "customer_desc" : "customer";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            
            ViewData["CurrentFilter"] = searchString;


            var warehouseContext = _context.Evidences.Include(e => e.Customer).Include(e => e.Item).Include(e => e.Warehouse);
            var evidences = from e in warehouseContext
                            select e;
            
            if (!String.IsNullOrEmpty(searchString))
            {
                Int64 anumber = 9999999;
                if (Int64.TryParse(searchString,out anumber)) {}
                evidences = evidences.Where(i => (i.WarehouseID == anumber && anumber != 9999999)
                                || (i.CustomerID == anumber && anumber != 9999999)
                                || (i.ItemID == anumber && anumber != 9999999)
                                );
            }


            switch (sortOrder)
            {
                case "date_desc":
                    evidences = evidences.OrderByDescending(i => i.Out);
                    break;
                case "item":
                    evidences = evidences.OrderBy(i => i.Item);
                    break;
                case "item_desc":
                    evidences = evidences.OrderByDescending(i => i.Item);
                    break;
                case "customer":
                    evidences = evidences.OrderBy(i => i.Customer);
                    break;
                case "customer_desc":
                    evidences = evidences.OrderByDescending(i => i.Customer);
                    break;
                case "warehouse":
                    evidences = evidences.OrderBy(i => i.Warehouse);
                    break;
                case "warehouse_desc":
                    evidences = evidences.OrderByDescending(i => i.Warehouse);
                    break;
                default:
                    evidences = evidences.OrderBy(i => i.Out);
                    break;
            }

            int pageSize = 6;
            return View(await PaginatedList<Evidence>.CreateAsync(evidences.AsNoTracking(), pageNumber ?? 1, pageSize));
        
        }

        // GET: Evidence/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evidence = await _context.Evidences
                .Include(e => e.Customer)
                .Include(e => e.Item)
                .Include(e => e.Warehouse)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (evidence == null)
            {
                return NotFound();
            }

            return View(evidence);
        }

        // GET: Evidence/Create
        public IActionResult Create()
        {
            //ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "ID");
            ViewData["CustomerID"]= new SelectList((from c in _context.Customers.ToList() select new {
                ID_Value = c.ID,
                FullName = c.FirstName + " " + c.LastName
            }), "ID_Value", "FullName");
            //ViewData["ItemID"] = new SelectList(_context.Items, "ID", "ID");
            ViewData["ItemID"] = new SelectList(_context.Items, nameof(Item.ID), nameof(Item.Name));
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, nameof(Warehouse.ID), nameof(Warehouse.Address));
            return View();
        }

        // POST: Evidence/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ItemID,WarehouseID,CustomerID,Out")] Evidence evidence)
        {
            if (ModelState.IsValid)
            {
                _context.Add(evidence);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "ID", evidence.CustomerID);
            ViewData["ItemID"] = new SelectList(_context.Items, "ID", "ID", evidence.ItemID);
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "ID", "ID", evidence.WarehouseID);
            return View(evidence);
        }

        // GET: Evidence/Edit/5
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evidence = await _context.Evidences.FindAsync(id);
            if (evidence == null)
            {
                return NotFound();
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "ID", evidence.CustomerID);
            ViewData["ItemID"] = new SelectList(_context.Items, "ID", "ID", evidence.ItemID);
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "ID", "ID", evidence.WarehouseID);
            return View(evidence);
        }

        // POST: Evidence/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ItemID,WarehouseID,CustomerID,Out")] Evidence evidence)
        {
            if (id != evidence.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evidence);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EvidenceExists(evidence.ID))
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
            ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "ID", evidence.CustomerID);
            ViewData["ItemID"] = new SelectList(_context.Items, "ID", "ID", evidence.ItemID);
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "ID", "ID", evidence.WarehouseID);
            return View(evidence);
        }

        // GET: Evidence/Delete/5
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evidence = await _context.Evidences
                .Include(e => e.Customer)
                .Include(e => e.Item)
                .Include(e => e.Warehouse)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (evidence == null)
            {
                return NotFound();
            }

            return View(evidence);
        }

        // POST: Evidence/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evidence = await _context.Evidences.FindAsync(id);
            _context.Evidences.Remove(evidence);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EvidenceExists(int id)
        {
            return _context.Evidences.Any(e => e.ID == id);
        }
    }
}
