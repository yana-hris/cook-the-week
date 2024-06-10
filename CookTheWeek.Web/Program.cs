namespace CookTheWeek.Web
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewEngines;
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Data.Models;
    using Infrastructure.ModelBinders;
    using Infrastructure.Extensions;
    using Infrastructure.BackgroundServices;
    using Infrastructure.HostedServices;
    using Rotativa.AspNetCore;
    using Services.Data.Interfaces;

    using static Common.GeneralApplicationConstants;

    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddUserSecrets<Program>();

            string? connectionString = builder.Configuration["ConnectionStrings:CookTheWeekDbContextConnection"];
            builder.Services.AddDbContext<CookTheWeekDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
                options.EnableSensitiveDataLogging();
            });
                

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
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<CookTheWeekDbContext>();


            builder.Services.AddHttpClient();
            builder.Services.AddApplicationServices(typeof(IRecipeService));

            // Register the warm-up service
            builder.Services.AddHostedService<WarmUpService>();

            builder.Services.AddSingleton<ICompositeViewEngine, CompositeViewEngine>();
            builder.Services.AddHostedService<UpdateMealPlansStatusService>();

            builder.Services.AddMemoryCache();
            builder.Services.AddResponseCaching();

            builder.Services.ConfigureApplicationCookie(cfg =>
            {
                cfg.LoginPath = "/User/Login";
                cfg.AccessDeniedPath = "/Home/Error/401";
            });

            builder.Services.AddLogging(options =>
            {
                options.AddConsole();
                options.AddDebug();
            });

            builder.Services.AddControllersWithViews()
                .AddMvcOptions(options =>
                {
                    options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
                    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
                });

            

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("DevelopmentCorsPolicy", builder =>
                {
                    builder.WithOrigins("https://localhost:7279")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
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
                app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // CORS middleware should be placed before routing middleware
            app.UseCors("DevelopmentCorsPolicy");

            app.UseRouting();

            app.UseAuthentication();

            app.UseResponseCaching();

            app.UseAuthorization();

            app.EnableOnlineUsersCheck();
           
            if(app.Environment.IsDevelopment())
            {
                app.SeedAdministrator(AdminUserUsername);
            }

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=HomeAdmin}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.UseRotativa();

            app.Run();
        }
    }
}
