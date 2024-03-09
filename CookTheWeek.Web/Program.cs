namespace CookTheWeek.Web
{
    using Microsoft.EntityFrameworkCore;
    
    using Data;
    using Data.Models;
    using Infrastructure.Extensions;
    using Infrastructure.ModelBinders;
    using Services.Interfaces;

    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            
            string? connectionString = builder.Configuration["CookTheWeek:ConnectionString"];
            builder.Services.AddDbContext<CookTheWeekDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");
                options.Password.RequireLowercase = builder.Configuration.GetValue<bool>("Password:RequireLowercase");
                options.Password.RequireUppercase = builder.Configuration.GetValue<bool>("Password:RequireUppercase");
                options.Password.RequireNonAlphanumeric = builder.Configuration.GetValue<bool>("Password:RequireNonAlphanumeric");
                options.Password.RequireDigit = builder.Configuration.GetValue<bool>("Password:RequireDigit");
                options.Password.RequiredLength = builder.Configuration.GetValue<int>("Password:RequiredLength");
            })
                .AddEntityFrameworkStores<CookTheWeekDbContext>();

            builder.Services.AddApplicationServices(typeof(IRecipeService));

            builder.Services.AddControllersWithViews()
                .AddMvcOptions(options =>
                {
                    options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
                });

            WebApplication app = builder.Build();
            
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error/500");
                app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");

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
