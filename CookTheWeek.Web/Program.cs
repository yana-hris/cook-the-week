namespace CookTheWeek.Web
{

    using Hangfire;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewEngines;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
    using CookTheWeek.Web.Infrastructure.ModelBinders;

    using static Common.GeneralApplicationConstants;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            ConfigurationManager config = builder.Configuration;
            config.AddUserSecrets<Program>();

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

            builder.Services.AddTransient<IEmailSender, EmailSender>();

            builder.Services.AddScoped<IUserContext, UserContext>();
            builder.Services.AddScoped<IIngredientAggregatorHelper, IngredientAggregatorHelper>();
            builder.Services.AddScoped<IRecipeSoftDeletedEventHandler, RecipeSoftDeletedEventHandler>();
            builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();


            // Register all Application Services using Extension method based on Reflection

            builder.Services.AddScoped<ICategoryRepository<RecipeCategory>, CategoryRepository<RecipeCategory>>();
            builder.Services.AddScoped<ICategoryRepository<IngredientCategory>, CategoryRepository<IngredientCategory>>();

            var suffixes = new[] { "Repository", "Service", "Factory" };
            var assemblyTypes = new[] { typeof(RecipeRepository).Assembly,
                        typeof(RecipeService).Assembly };
           
            builder.Services.AddServicesByConvention(
                assemblyTypes,
                suffixes);

            builder.Services.AddHttpContextAccessor();
           
            builder.Services.AddSingleton<ICompositeViewEngine, CompositeViewEngine>();
            builder.Services.AddHostedService<UpdateMealPlansStatusService>();

            // Add Hangfire for scheduled jobs (mealplan claims update)
            builder.Services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connectionString));
            builder.Services.AddHangfireServer();


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


            builder.Services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddDebug();
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

            builder.Services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
            
            builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder.AllowAnyHeader()
                           .AllowAnyMethod()
                           .SetIsOriginAllowed((host) => true)
                           .AllowCredentials();
                }));

            // Register the SendGrid EmailSender service
            builder.Services.Configure<SendGridClientOptions>(config.GetSection("SendGrid"));
                        
            WebApplication app = builder.Build();

            await app.Services.ApplyMigrationsAndSeedData(true);

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();

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
            app.UseCors("CorsPolicy");

            if (app.Environment.IsDevelopment())
            {
                app.SeedAdministrator(AdminUserUsername);
            }

            app.UseRouting();

            // Authentication middleware
            app.UseAuthentication();

            // Custom middleware for retrieving the userId
            app.EnableUserContext();

            // Authorization middleware
            app.UseAuthorization();

            // Custom middleware for checking online users (requires user ID)
            app.EnableOnlineUsersCheck();

            // Optional: Use Hangfire Dashboard for job monitoring
            app.UseHangfireDashboard("/hangfire");
            app.RegisterRecurringJobs();

            app.MapControllerRoute(
                name: $"{AdminAreaName}",
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
