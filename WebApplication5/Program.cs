using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication5.Data;

namespace WebApplication5
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequiredLength = 2;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();
                
            builder.Services.AddRazorPages();

            var app = builder.Build();

			try
			{
				using var scope = app.Services.CreateScope();
				var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

				context.Database.EnsureCreated();

				if (!context.Users.Where(x => x.UserName == "admin@gmail.com").Any())
				{
					var superAdminUser = new IdentityUser
					{
						Email = "admin@gmail.com",
						UserName = "admin",
						EmailConfirmed = true,
					};

					userManager.CreateAsync(superAdminUser, "123456").GetAwaiter().GetResult();

					//Roles contains user, user contains claims
					var adminClaim = new Claim("Role", "Admin");

					userManager.AddClaimAsync(superAdminUser, adminClaim).GetAwaiter().GetResult();
				}

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
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

            app.Run();
        }
    }
}