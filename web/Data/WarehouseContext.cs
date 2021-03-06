using web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace web.Data
{
    public class WarehouseContext : IdentityDbContext<ApplicationUser>
    {
        public WarehouseContext(DbContextOptions<WarehouseContext> options) : base(options)
        {
        }

        public DbSet<Warehouse> Warehouses { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Distributor> Distributors { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Evidence> Evidences { get; set; }        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Warehouse>().ToTable("Warehouse");
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Distributor>().ToTable("Distributor");
            modelBuilder.Entity<Item>().ToTable("Item");
            modelBuilder.Entity<Evidence>().ToTable("Evidence");
        }
    }
}