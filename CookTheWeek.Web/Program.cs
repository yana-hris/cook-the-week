namespace CookTheWeek.Web
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewEngines;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Serialization;
    using Rotativa.AspNetCore;
    using SendGrid;

    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Events.Dispatchers;
    using CookTheWeek.Services.Data.Events.EventHandlers;
    using CookTheWeek.Services.Data.Helpers;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Services.Data.Services;
    using CookTheWeek.Web.Infrastructure.BackgroundServices;
    using CookTheWeek.Web.Infrastructure.Extensions;
    using CookTheWeek.Web.Infrastructure.HostedServices;
    using CookTheWeek.Web.Infrastructure.ModelBinders;
    using CookTheWeek.Web.Infrastructure.Middlewares;

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

            builder.Services.AddScoped<IUserContext, UserContext>();
            builder.Services.AddScoped<IIngredientAggregatorHelper, IngredientAggregatorHelper>();
            builder.Services.AddScoped<IRecipeSoftDeletedEventHandler, RecipeSoftDeletedEventHandler>();
            builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();


            var suffixes = new[] { "Repository", "Service", "Factory" };
            var assemblyTypes = new[] { typeof(RecipeRepository).Assembly,
                        typeof(RecipeService).Assembly };

            //// Register all services and repositories from multiple assemblies
            //builder.Services.AddApplicationServicesOfType(assemblyTypes, suffixes);
            builder.Services.AddServicesByConvention(
                assemblyTypes,
                suffixes);

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

            builder.Services.AddControllersWithViews();

            // Add session services
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
                options.Cookie.HttpOnly = true;  // Make the session cookie HttpOnly for security
                options.Cookie.IsEssential = true; // Ensure cookie is not affected by consent policies
            });

            // Add distributed memory cache (used by session to store data)
            builder.Services.AddDistributedMemoryCache();

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

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/InternalServerError");
                app.UseStatusCodePagesWithReExecute("/Home/NotFound", "?code={0}");
                app.UseHsts();  // HSTS for production
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            // CORS should be placed before routing
            app.UseCors("CorsPolicy");

            app.UseRouting();

            // Authentication middleware
            app.UseAuthentication();

            // Custom middleware for retrieving the userId
            app.UseMiddleware<UserContextMiddleware>();

            // Authorization middleware
            app.UseAuthorization();

            // Custom middleware for checking online users (requires user ID)
            app.EnableOnlineUsersCheck();

            // Session middleware
            app.UseSession();

            if (app.Environment.IsDevelopment())
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
