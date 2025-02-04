namespace CookTheWeek.Web
{

    using CloudinaryDotNet;
    using Hangfire;
    using Hangfire.SqlServer;
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
    using CookTheWeek.Services.Data.Events;
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

            if (builder.Environment.IsDevelopment())
            {
                try
                {
                    config.AddUserSecrets<Program>();
                }
                catch (Exception)
                {
                    Console.WriteLine("User Secrets not found. Falling back to appsettings.json.");
                }
            }

            var rotativaPath =Path.GetFullPath(builder.Environment.WebRootPath);            
            RotativaConfiguration.Setup(rotativaPath);

            string? connectionString = config.GetConnectionString("CookTheWeekDbContextConnection");
            builder.Services.AddDbContext<CookTheWeekDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
                if (builder.Environment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging();
                }
            });

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            }

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
            builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            builder.Services.AddScoped<IDomainEventHandler<RecipeSoftDeletedEvent>, RecipeSoftDeletedEventHandler>();
            builder.Services.AddScoped<IDomainEventHandler<RecipeImageUpdateEvent>, RecipeImageUpdateEventHandler>();


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
            builder.Services.AddApplicationInsightsTelemetry();


            builder.Services.AddSingleton<ICompositeViewEngine, CompositeViewEngine>();
            builder.Services.AddHostedService<UpdateMealPlansStatusService>();

            // Add Hangfire for scheduled jobs (mealplan claims update)
            builder.Services.AddHangfire(configuration =>
            {
                configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
                {
                    QueuePollInterval = TimeSpan.FromHours(1), // Poll once per hour
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(120), // Lock timeout
                    JobExpirationCheckInterval = TimeSpan.FromDays(7), // Cleanup weekly
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SchemaName = "Hangfire",
                    EnableHeavyMigrations = false,
                    CountersAggregateInterval = TimeSpan.FromHours(6), // Aggregate stats every 6 hours
                    DashboardJobListLimit = 100, // Limit job history to 100

                });
            });

            builder.Services.AddHangfireServer(options =>
            {
                // Adjusting workers count for fewer threads to reduce database load
                options.WorkerCount = 1;
                options.ServerTimeout = TimeSpan.FromMinutes(30);
            });


            builder.Services.AddMemoryCache();
            builder.Services.AddResponseCaching();

            builder.Services.ConfigureApplicationCookie(cfg =>
            {
                cfg.Cookie.HttpOnly = true;               

                if (builder.Environment.IsDevelopment())
                {
                    cfg.Cookie.SecurePolicy = CookieSecurePolicy.None;
                }
                else
                {
                    cfg.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                }

                cfg.ExpireTimeSpan = TimeSpan.FromDays(3);
                cfg.SlidingExpiration = true;
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

                if (builder.Environment.IsDevelopment())
                {
                    logging.AddConsole();
                    logging.AddDebug();
                }

                logging.AddApplicationInsights(
                    configureTelemetryConfiguration: (config) => { },
                    configureApplicationInsightsLoggerOptions: (options) => { }
                );
                
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

            builder.Services.AddSingleton(x =>
            {
                var cloudinaryConfig = config.GetSection("Cloudinary");
                var cloudinary = new Cloudinary(new Account(cloudinaryConfig["CloudName"], cloudinaryConfig["ApiKey"], cloudinaryConfig["ApiSecret"]));
                cloudinary.Api.Secure = true;

                return cloudinary;
            });

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
                    builder =>
                    {
                        builder.AllowAnyHeader()
                               .AllowAnyMethod()
                               .SetIsOriginAllowed((host) => true) // Allow all in development
                               .AllowCredentials();
                    }));
            }
            else
            {
                builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins("https://cooktheweek.azurewebsites.net") // Replace with your production domain
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    }));
            }

            // Register the SendGrid EmailSender service
            builder.Services.Configure<SendGridClientOptions>(config.GetSection("SendGrid"));
                        
            WebApplication app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
                await app.Services.ApplyMigrationsAndSeedData(true); // Seeds data and applies migrations when set to true

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
            
            app.SeedAdministrator(); // Works for both Dev & Prod env. using configuration settings

            app.UseRouting();

            // Authentication middleware
            app.UseAuthentication();

            // Custom middleware for retrieving the userId
            app.EnableUserContext();

            // Authorization middleware
            app.UseAuthorization();

            // Custom middleware for checking online users (requires user ID)
            app.EnableOnlineUsersCheck();

            // Hangfire Dashboard for job monitoring
            app.UseHangfireDashboard("/hangfire");
            app.RegisterRecurringJobs();
            //app.RegisterScheduledJobs();

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
