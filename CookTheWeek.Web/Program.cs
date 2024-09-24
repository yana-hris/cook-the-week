namespace CookTheWeek.Web
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewEngines;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Serialization;
    using Rotativa.AspNetCore;
    using SendGrid;

    using Data;
    using Data.Models;
    using Data.Repositories;
    using Infrastructure.BackgroundServices;
    using Infrastructure.Extensions;
    using Infrastructure.HostedServices;
    using Infrastructure.ModelBinders;
    using Services.Data.Factories.Interfaces;
    using Services.Data.Services.Interfaces;
    using Services.Data.Services;

    using static Common.GeneralApplicationConstants;

    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddUserSecrets<Program>();
            ConfigurationManager config = builder.Configuration;

            string? connectionString = config["ConnectionStrings:CookTheWeekDbContextConnection"];
            builder.Services.AddDbContext<CookTheWeekDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
                options.EnableSensitiveDataLogging();                
            });
                

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                IConfigurationSection passwordSection =
                   config.GetSection("Identity:Password");                
                options.SignIn.RequireConfirmedAccount = builder.Configuration
                    .GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");
                options.Password.RequireLowercase = passwordSection
                    .GetValue<bool>("RequireLowercase");
                options.Password.RequireUppercase = passwordSection
                    .GetValue<bool>("RequireUppercase");
                options.Password.RequireNonAlphanumeric = passwordSection
                    .GetValue<bool>("RequireNonAlphanumeric");
                options.Password.RequireDigit = passwordSection
                    .GetValue<bool>("RequireDigit");
                options.Password.RequiredLength = passwordSection
                    .GetValue<int>("RequiredLength");

                IConfigurationSection lockoutSection =
                   config.GetSection("Identity:Lockout");
                options.Lockout.AllowedForNewUsers = lockoutSection
                    .GetValue<bool>("AllowedForNewUsers");
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan
                    .FromMinutes(lockoutSection.GetValue<double>("DefaultLockoutTimeSpan"));
                options.Lockout.MaxFailedAccessAttempts = lockoutSection.
                    GetValue<int>("MaxFailedAccessAttempts");
            })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<CookTheWeekDbContext>();
           
            // Add Authentication services with Google OAuth
            builder.Services.AddAuthentication()
               .AddGoogle(options =>
               {
                   IConfigurationSection googleAuthNSection =
                   config.GetSection("Authentication:Google");
                   options.ClientId = googleAuthNSection["ClientId"];
                   options.ClientSecret = googleAuthNSection["ClientSecret"];
                   options.AccessDeniedPath = "/User/AccessDeniedPathInfo";
               })
               .AddFacebook(options =>
               {
                   IConfigurationSection facebookAuthNSection =
                   config.GetSection("Authentication:Facebook");
                   options.ClientId = facebookAuthNSection["AppId"];
                   options.ClientSecret = facebookAuthNSection["AppSecret"];
                   options.AccessDeniedPath = "/User/AccessDeniedPathInfo";
               });  

            builder.Services.AddHttpClient();
            // Register repositories
            builder.Services.AddApplicationTypes(typeof(IRecipeRepository), "Repository");

            // Register factories
            builder.Services.AddApplicationTypes(typeof(IRecipeViewModelFactory), "Factory");

            // Register repositories
            builder.Services.AddApplicationTypes(typeof(IRecipeService), "Service");

            builder.Services.AddHttpContextAccessor();  

            builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder.AllowAnyHeader()
                       .AllowAnyMethod()
                       .SetIsOriginAllowed((host) => true)
                       .AllowCredentials();
            }));

            // Register the warm-up service
            builder.Services.AddHostedService<WarmUpService>();

            builder.Services.AddSingleton<ICompositeViewEngine, CompositeViewEngine>();
            builder.Services.AddHostedService<UpdateMealPlansStatusService>();

            builder.Services.AddMemoryCache();
            builder.Services.AddResponseCaching();

            builder.Services.ConfigureApplicationCookie(cfg =>
            {                
                cfg.LoginPath = "/User/Login";
                cfg.AccessDeniedPath = "/User/AccessDeniedPathInfo";

            });
           
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
                options.ConsentCookieValue = "true";
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
                    options.ModelBinderProviders.Insert(1, new SanitizingModelBinderProvider());
                    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
                }).AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder.AllowAnyHeader()
                       .AllowAnyMethod()
                       .SetIsOriginAllowed((host) => true)
                       .AllowCredentials();
            }));

            // Register the SendGrid EmailSender service
            builder.Services.Configure<SendGridClientOptions>(builder.Configuration.GetSection("SendGrid"));
            builder.Services.AddTransient<IEmailSender, EmailSender>();

            
            WebApplication app = builder.Build();

            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseMigrationsEndPoint();
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
                app.UseExceptionHandler("/Home/InternalServerError");
                app.UseStatusCodePagesWithReExecute("/Home/NotFound", "?code={0}");
                app.UseHsts();
            //}

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            // CORS middleware should be placed before routing middleware
            //app.UseCors("DevelopmentCorsPolicy");
            app.UseCors("CorsPolicy");

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
