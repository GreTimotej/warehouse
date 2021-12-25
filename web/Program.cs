using web.Data;
using web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AzureContext");

// Add services to the container.
builder.Services.AddControllersWithViews();

// builder.Services.AddDbContext<WarehouseContext>(options =>
//             options.UseSqlServer(builder.Configuration.GetConnectionString("WarehouseContext")));
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<WarehouseContext>();
builder.Services.AddDbContext<WarehouseContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddSwaggerGen();

var app = builder.Build();
CreateDbIfNotExists(app);

// await CreateRoles(IServiceProvider);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.Run();

static void CreateDbIfNotExists(IHost host)
{
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<WarehouseContext>();
            //context.Database.EnsureCreated();
            DbInitializer.Initialize(context);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating the DB.");
        }
    }
}

// async Task CreateRoles(IServiceProvider serviceProvider)
// {
//     //initializing custom roles 
//     var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//     var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
//     string[] roleNames = { "Admin", "Manager", "Staff" };
//     IdentityResult roleResult;

//     foreach (var roleName in roleNames)
//     {
//         var roleExist = await RoleManager.RoleExistsAsync(roleName);
//         // ensure that the role does not exist
//         if (!roleExist)
//         {
//             //create the roles and seed them to the database: 
//             roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
//         }
//     }

//     // find the user with the admin email 
//     var _user = await UserManager.FindByEmailAsync("admin@warehouses.com");

//     // check if the user exists
//     if(_user == null)
//     {
//         //Here you could create the super admin who will maintain the web app
//         var poweruser = new ApplicationUser
//         {
//             UserName = "Admin",
//             Email = "admin@warehouses.com",
//         };
//         string adminPassword = "Test123_";

//         var createPowerUser = await UserManager.CreateAsync(poweruser, adminPassword);
//         if (createPowerUser.Succeeded)
//         {
//             //here we tie the new user to the role
//             await UserManager.AddToRoleAsync(poweruser, "Admin");

//         }
//     }
// }
