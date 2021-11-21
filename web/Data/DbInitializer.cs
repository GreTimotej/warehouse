using web.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;

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
            new Warehouse{Address="Ljubljanska cesta 5", ZIP=1000, City="Ljubljana", Country="Slovenia"},
            new Warehouse{Address="Šišenska cesta 13", ZIP=1000, City="Ljubljana", Country="Slovenia"},
            new Warehouse{Address="Dunajska cesta 227", ZIP=1000, City="Ljubljana", Country="Slovenia"}
            };
            foreach (Warehouse w in warehouses)
            {
                context.Warehouses.Add(w);
            }
            context.SaveChanges();

            var distributors = new Distributor[]
            {
            new Distributor{Name="Mouser Electronix", Address="Wunstrasse 1143", ZIP=80331, City="Munchen", Country="Germany"},
            new Distributor{Name="Norbert Electricity", Address="West Avenue 1143", ZIP=64030, City="Kansas City", Country="Msoury"},
            new Distributor{Name="Vijaki Viktor", Address="Brezje 13", ZIP=4243, City="Brezje", Country="Slovenia"},
            new Distributor{Name="Acovia Design šotori", Address="Ljubljanska cesta 55", ZIP=1230, City="Domžale", Country="Slovenia"},
            };
            foreach (Distributor d in distributors)
            {
                context.Distributors.Add(d);
            }
            context.SaveChanges();

            var customers = new Customer[]
            {
            new Customer{FirstName="Janez", LastName="Novak", Address="Razkrižje 5", ZIP=9240, City="Razkrižje", Country="Slovenia"},
            new Customer{FirstName="John", LastName="Smith", Address="Black street", ZIP=533544, City="London", Country="United Kingdom"},
            
            };
            foreach (Customer c in customers)
            {
                context.Customers.Add(c);
            }
            context.SaveChanges();

            var items = new Item[]
            {
                new Item{Name="100 Ohm THT resistor", Quantity=19, WarehouseID=2, CustomerID=2},
                new Item{Name="M5 screw kit", Description="M5 screw with bolt and nuts. Inox material", WarehouseID=2, CustomerID=2},
                new Item{Name="A4 Paper", Description="Paper",Quantity=13, WarehouseID=2, CustomerID=2},
                new Item{Name="B4 Paper", Description="Paper",Quantity=13, WarehouseID=2, CustomerID=2},
            };
            foreach (Item i in items)
            {
                context.Items.Add(i);
            }
            context.SaveChanges();

            var roles = new IdentityRole[] {
                new IdentityRole{Id="1", Name="Administrator"},
                new IdentityRole{Id="2", Name="Manager"},
                new IdentityRole{Id="3", Name="Staff"}
            };

            foreach (IdentityRole r in roles)
            {
                context.Roles.Add(r);
            }

            var user = new ApplicationUser
            {
                FirstName = "A",
                LastName = "A",
                Email = "admin@warehouse.com",
                UserName = "admin@warehouse.com",
                NormalizedUserName = "admin@warehouse.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user,"Test123!");
                user.PasswordHash = hashed;
                context.Users.Add(user);
                
            }

            context.SaveChanges();

            var manager = new ApplicationUser
            {
                FirstName = "John",
                LastName = "Smithy",
                Email = "john.smith@warehouse.com",
                UserName = "john.smith@warehouse.com",
                NormalizedUserName = "john.smith@warehouse.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            if (!context.Users.Any(u => u.UserName == manager.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(manager,"Test123!");
                manager.PasswordHash = hashed;
                context.Users.Add(manager);
                
            }

            context.SaveChanges();

            var staff = new ApplicationUser
            {
                FirstName = "Miha",
                LastName = "novak",
                Email = "miha.novak@warehouse.com",
                UserName = "miha.novak@warehouse.com",
                NormalizedUserName = "miha.novak@warehouse.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            if (!context.Users.Any(u => u.UserName == staff.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(staff,"Test123!");
                staff.PasswordHash = hashed;
                context.Users.Add(staff);
                
            }

            context.SaveChanges();

            var UserRoles = new IdentityUserRole<string>[]
            {
                new IdentityUserRole<string>{RoleId = roles[0].Id, UserId=user.Id},
                new IdentityUserRole<string>{RoleId = roles[1].Id, UserId=manager.Id},
                new IdentityUserRole<string>{RoleId = roles[2].Id, UserId=staff.Id},
            };

            foreach (IdentityUserRole<string> r in UserRoles)
            {
                context.UserRoles.Add(r);
            }

            context.SaveChanges();
        }
    }
}