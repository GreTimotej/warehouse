using web.Models;
using System;
using System.Linq;

namespace web.Data
{
    public static class DbInitializer
    {
        public static void Initialize(WarehouseContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Warehouses.Any())
            {
                return;   // DB has been seeded
            }

            var warehouses = new Warehouse[]
            {
            new Warehouse{ID=1, Address="Ljubljanska cesta 5", ZIP=1000, City="Ljubljana", Country="Slovenia"},
            new Warehouse{ID=2, Address="Šišenska cesta 13", ZIP=1000, City="Ljubljana", Country="Slovenia"},
            new Warehouse{ID=3, Address="Dunajska cesta 227", ZIP=1000, City="Ljubljana", Country="Slovenia"}
            };
            foreach (Warehouse w in warehouses)
            {
                context.Warehouses.Add(w);
            }
            context.SaveChanges();

            var distributors = new Distributor[]
            {
            new Distributor{ID=1, Name="Mouser Electronix", Address="Wunstrasse 1143", ZIP=80331, City="Munchen", Country="Germany"},
            new Distributor{ID=2, Name="Norbert Electricity", Address="West Avenue 1143", ZIP=64030, City="Kansas City", Country="Msoury"},
            new Distributor{ID=3, Name="Vijaki Viktor", Address="Brezje 13", ZIP=4243, City="Brezje", Country="Slovenia"},
            new Distributor{ID=4, Name="Acovia Design šotori", Address="Ljubljanska cesta 55", ZIP=1230, City="Domžale", Country="Slovenia"},
            };
            foreach (Distributor d in distributors)
            {
                context.Distributors.Add(d);
            }
            context.SaveChanges();

            var customers = new Customer[]
            {
            new Customer{ID=1, FirstName="Janez", LastName="Novak", Address="Razkrižje 5", ZIP=9240, City="Razkrižje", Country="Slovenia"},
            new Customer{ID=2, FirstName="John", LastName="Smith", Address="Black street", ZIP=533544, City="London", Country="United Kingdom"},
            
            };
            foreach (Customer c in customers)
            {
                context.Customers.Add(c);
            }
            context.SaveChanges();

            var items = new Item[]
            {
                new Item{ID=1, Name="100 Ohm THT resistor", WarehouseID=2, CustomerID=2},
                new Item{ID=2, Name="M5 screw kit", Description="M5 screw with bolt and nuts. Inox material", WarehouseID=2, CustomerID=2},
            };
            foreach (Item i in items)
            {
                context.Items.Add(i);
            }
            context.SaveChanges();
        }
    }
}