using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using ShipmentManagement.Data;
using ShipmentManagement.Repositories;
using ShipmentManagement.Repositories.Implementations;
using ShipmentManagement.Repositories.Interfaces;
using ShipmentManagement.Services;
using ShipmentManagement.Services.Implementations;
using ShipmentManagement.Services.Interfaces;

namespace ShipmentManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                )
            );

            // Auth Filter 
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<ShipmentManagement.Filters.AuthFilter>();
            });

            builder.Services.AddEndpointsApiExplorer();

            // Session 
            builder.Services.AddDistributedMemoryCache(); // Required for session state management 
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);  // Session timeout after 60 minutes of inactivity
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            builder.Services.AddHttpContextAccessor();  // Required to access HttpContext in services 


            // Repositories
            builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
            builder.Services.AddScoped<IShipmentRepository, ShipmentRepository>();
            builder.Services.AddScoped<IShipRepository, ShipRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IPortRepository, PortRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // Services
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<IShipmentService, ShipmentService>();
            builder.Services.AddScoped<IShipService, ShipService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ILogActionService, LogActionService>();


            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                            .AddCookie(options =>
                            {
                                options.LoginPath = "/Account/Login";
                                options.AccessDeniedPath = "/Account/AccessDenied";
                                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Dashboard/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();  

            app.MapControllerRoute(
                name: "default",
               // pattern: "{controller=Dashboard}/{action=Index}/{id?}");
                pattern: "{controller=Account}/{action=Login}/{id?}"
                );

            app.MapControllers();  //Api Controllers
            app.Run();
        }
    }
}
