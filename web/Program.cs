using web.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<WarehouseContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("WarehouseContext")));


var app = builder.Build();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// static void CreateDbIfNotExists(IHost host)
// {
//     using (var scope = host.Services.CreateScope())
//     {
//         var services = scope.ServiceProvider;
//         try
//         {
//             var context = services.GetRequiredService<WarehouseContext>();
//             //context.Database.EnsureCreated();
//             DbInitializer.Initialize(context);
//         }
//         catch (Exception ex)
//         {
//             var logger = services.GetRequiredService<ILogger<Program>>();
//             logger.LogError(ex, "An error occurred creating the DB.");
//         }
//     }
// }
