using Microsoft.EntityFrameworkCore;// Add Entity Framework Core services like database context and migrations for the application
using SmartRecruitment.Data;// Add the application data access layer for interacting with the database

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();// Add MVC services

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    );
});// Add DbContext with MySQL connection

builder.Services.AddDistributedMemoryCache();// Add distributed memory cache for session management like storing session data in memory for the application

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});// Add session services with a 30-minute idle timeout

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}// Use exception handler and HSTS in non-development environments

app.UseHttpsRedirection();// Redirect HTTP requests to HTTPS

app.UseStaticFiles();// Serve static files for the application used by the application, such as CSS, JavaScript, and images

app.UseRouting();// Enable routing for the application like mapping incoming requests to the appropriate controllers and actions

app.UseSession();// Enable session management for the application

app.UseAuthorization();// Enable authorization for the application like checking if the user is authenticated and authorized to access certain resources

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();// Start the application and listen for incoming requests