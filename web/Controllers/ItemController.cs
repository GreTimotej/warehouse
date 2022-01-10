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
        public async Task<IActionResult> Index(
        string sortOrder,
        string currentFilter,
        string searchString,
        // bool activeBool,
        int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DescriptionSortParm"] = sortOrder == "description" ? "description_desc" : "description";
            ViewData["CustomerSortParm"] = sortOrder == "customer" ? "customer_desc" : "customer";
            ViewData["QuantitySortParm"] = sortOrder == "quantity" ? "quantity_desc" : "quantity";
            ViewData["WarehouseSortParm"] = sortOrder == "warehouse" ? "warehouse_desc" : "warehouse";
            ViewData["DistributorSortParm"] = sortOrder == "distributor" ? "distributor_desc" : "distributor";
            ViewData["ActiveSortParm"] = sortOrder == "active" ? "active_desc" : "active";



            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            
            ViewData["CurrentFilter"] = searchString;

            var warehouseContext = _context.Items.Include(i => i.Customer).Include(i => i.Distributor).Include(i => i.Warehouse);
            var items = from i in warehouseContext
                        select i;


            if (!String.IsNullOrEmpty(searchString))
            {
                Int64 anumber = 9999999;
                if (Int64.TryParse(searchString,out anumber)) {}
                items = items.Where(i => i.Name.Contains(searchString)
                                || i.Description.Contains(searchString)
                                || (i.Quantity == anumber && anumber != 9999999)
                                || (i.CustomerID == anumber && anumber != 9999999)
                                || (i.WarehouseID == anumber && anumber != 9999999)
                                || (i.DistributorID == anumber && anumber != 9999999));
            }

            // ViewData["ActiveFilter"] = activeBool;

            // switch (activeBool)
            // {
            //     case true:
            //         items = items.Where(i => i.Active == true);
            //         break;
            //     case false:
            //         items = items.Where(i => i.Active == false);
            //         break;
            //     default:
            //         items = items.Where(i => i.Active == true && i.Active == false);
            //         break;
            // }

            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(i => i.Name);
                    break;
                case "description":
                    items = items.OrderBy(i => i.Description);
                    break;
                case "description_desc":
                    items = items.OrderByDescending(i => i.Description);
                    break;
                case "customer":
                    items = items.OrderBy(i => i.Customer);
                    break;
                case "customer_desc":
                    items = items.OrderByDescending(i => i.Customer);
                    break;
                case "quantity":
                    items = items.OrderBy(i => i.Quantity);
                    break;
                case "quantity_desc":
                    items = items.OrderByDescending(i => i.Quantity);
                    break;
                case "warehouse":
                    items = items.OrderBy(i => i.Warehouse);
                    break;
                case "warehouse_desc":
                    items = items.OrderByDescending(i => i.Warehouse);
                    break;
                case "distributor":
                    items = items.OrderBy(i => i.Distributor);
                    break;
                case "distributor_desc":
                    items = items.OrderByDescending(i => i.Distributor);
                    break;
                case "active":
                    items = items.OrderBy(i => i.Active);
                    break;
                case "active_desc":
                    items = items.OrderByDescending(i => i.Active);
                    break;
                default:
                    items = items.OrderBy(i => i.Name);
                    break;
            }

            
            int pageSize = 8;
            return View(await PaginatedList<Item>.CreateAsync(items.AsNoTracking(), pageNumber ?? 1, pageSize));
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
                .Include(i => i.Distributor)
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

            //ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "ID");
            //ViewData["DistributorID"] = new SelectList(_context.Distributors, "ID", "ID");
            //ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "ID", "ID");

            ViewData["CustomerID"]= new SelectList((from c in _context.Customers.ToList() select new {
                ID_Value = c.ID,
                FullName = c.FirstName + " " + c.LastName
            }), "ID_Value", "FullName");  
            ViewData["DistributorID"] = new SelectList(_context.Distributors, nameof(Distributor.ID),nameof(Distributor.Name));
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, nameof(Warehouse.ID), nameof(Warehouse.Address));
            return View();

        }

        // POST: Item/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,Quantity,Active,WarehouseID,CustomerID,DistributorID")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "ID", item.CustomerID);
            ViewData["DistributorID"] = new SelectList(_context.Distributors, "ID", "ID", item.DistributorID);
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

            //ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "ID", item.CustomerID);
            //ViewData["DistributorID"] = new SelectList(_context.Distributors, "ID", "ID", item.DistributorID);
            //ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "ID", "ID", item.WarehouseID);
                        ViewData["CustomerID"]= new SelectList((from c in _context.Customers.ToList() select new {
                ID_Value = c.ID,
                FullName = c.FirstName + " " + c.LastName
            }), "ID_Value", "FullName");  
            ViewData["DistributorID"] = new SelectList(_context.Distributors, nameof(Distributor.ID),nameof(Distributor.Name));
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, nameof(Warehouse.ID), nameof(Warehouse.Address));

            return View(item);
        }

        // POST: Item/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,Quantity,Active,WarehouseID,CustomerID,DistributorID")] Item item)
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
            ViewData["DistributorID"] = new SelectList(_context.Distributors, "ID", "ID", item.DistributorID);
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
                .Include(i => i.Distributor)
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


        // GET: Item/Ship/5
        [Authorize]
        public async Task<IActionResult> Ship(int? id)
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

        // POST: Item/Ship/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ship(int id)
        {
            var item = await _context.Items.FindAsync(id);
            item.Active = false;
            _context.Items.Update(item);
            await _context.SaveChangesAsync();

            var evidence = new Evidence
            {
                ItemID = item.ID,
                WarehouseID = item.WarehouseID,
                CustomerID = item.CustomerID,
                Out = DateTime.Now

            };
            _context.Add(evidence);
                await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ID == id);
        }
    }
}
