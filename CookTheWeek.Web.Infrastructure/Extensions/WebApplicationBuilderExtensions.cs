namespace CookTheWeek.Web.Infrastructure.Extensions
{
    using System.Reflection;

    using Hangfire;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.Infrastructure.Middlewares;

    using static CookTheWeek.Common.GeneralApplicationConstants;

    /// <summary>
    /// This method registers all services with their interfaces and implementations of a given Assembly
    /// The assembly is taken from the type of random service implementation.
    /// <param name="serviceType">Type of random service implementation</param>
    /// <exception cref="InvalidOperationException"></exception>
    /// </summary>
    public static class WebApplicationBuilderExtensions
    {
        /// <summary>
        /// Registers services in the specified assemblies by convention, based on matching class name suffixes. 
        /// This method scans each provided assembly for class types with specified suffixes and registers 
        /// them with their implemented interfaces as scoped services in the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="assembliesToScan">An array of assemblies to scan for class types to register.</param>
        /// <param name="suffixes">An array of suffix strings to match class names against (e.g., "Repository", "Service"). 
        /// Classes with names ending in these suffixes are registered.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> with the registered services.</returns>
        /// <remarks>
        /// This method supports both open and closed generic types. For example, if a class implements 
        /// <c>ICategoryService{T}</c>, it will be registered as a scoped service, allowing for dependency injection 
        /// of open generic interfaces. Non-generic classes and closed generic classes are registered directly 
        /// against their implemented interfaces. 
        /// </remarks>
        /// <example>
        /// <code>
        /// var assembliesToScan = new[] { typeof(RecipeService).Assembly };
        /// var suffixes = new[] { "Repository", "Service" };
        /// services.AddServicesByConvention(assembliesToScan, suffixes);
        /// </code>
        /// </example>

        public static IServiceCollection AddServicesByConvention(this IServiceCollection services,
                                                        Assembly[] assembliesToScan,
                                                        string[] suffixes)
        {
            foreach (var assembly in assembliesToScan)
            {
                // Get all class types in the assembly
                var typesToRegister = assembly.GetTypes()
                    .Where(type => type.IsClass && !type.IsAbstract) // Only classes
                    .Where(type => suffixes.Any(suffix => type.Name.EndsWith(suffix))) // Matching suffix
                    .ToList();

                foreach (var implementationType in typesToRegister)
                {
                    var interfaces = implementationType.GetInterfaces();

                    foreach (var implementedInterface in interfaces)
                    {
                        if (implementedInterface.IsGenericTypeDefinition && implementationType.IsGenericTypeDefinition)
                        {
                            // Register open generic types (e.g., ICategoryService<TCategory, TAddFormModel, ...>)
                            services.AddScoped(implementedInterface.GetGenericTypeDefinition(), implementationType.GetGenericTypeDefinition());
                        }
                        else
                        {
                            // Register closed generic types & non-generic services
                            services.AddScoped(implementedInterface, implementationType);
                        }
                    }
                }
            }

            return services;
        }
        
        /// <summary>
        /// This method seeds the first role of administrator upon database creation if environment is development and 
        /// if it does not exist
        /// </summary>
        /// <param name="app">The currecnt application</param>
        /// <param name="userName">The user we want to make admin`s username</param>
        /// <returns>IApplicationBuilder and allows chaining</returns>
        public static IApplicationBuilder SeedAdministrator(this IApplicationBuilder app, string userName)
        {
            //In order to get the DI services in a static class, we first need to get access to the Service provider, so we first create the scope
            using IServiceScope scopedServices = app.ApplicationServices.CreateScope();

            // In static classes this is the only way to get the service container (as it cannot be injected in the contructor)
            IServiceProvider serviceProvider = scopedServices.ServiceProvider;


            UserManager<ApplicationUser> userManager =
                serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            RoleManager<IdentityRole<Guid>> roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            // In static classes we cannot have async await pattern, so we need to use Tasks to have asynchronous code in synchronous static methods
            Task.Run(async () =>
            {
                try
                {
                    if (!await roleManager.RoleExistsAsync(AdminRoleName))
                    {
                        IdentityRole<Guid> role = new IdentityRole<Guid>(AdminRoleName);

                        var roleResult = await roleManager.CreateAsync(role);

                        if (!roleResult.Succeeded)
                        {
                            Console.WriteLine($"Failed to create role: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                            return;
                        }

                    }

                    ApplicationUser? adminUser = await userManager.FindByNameAsync(AdminUserUsername);

                    if (adminUser == null)
                    {
                        // Create the admin user
                        adminUser = new ApplicationUser
                        {
                            UserName = AdminUserUsername,
                            Email = AdminUserEmail,
                            EmailConfirmed = true // Assuming email confirmation isn't needed for the initial admin
                        };

                        var createResult = await userManager.CreateAsync(adminUser, AdminUserPassword);

                        if (!createResult.Succeeded)
                        {
                            Console.WriteLine($"Failed to create admin user: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                            return;
                        }
                    }

                    if (!await userManager.IsInRoleAsync(adminUser, AdminRoleName))
                    {
                        var addRoleResult = await userManager.AddToRoleAsync(adminUser, AdminRoleName);

                        if (!addRoleResult.Succeeded)
                        {
                            Console.WriteLine($"Failed to assign role to user: {string.Join(", ", addRoleResult.Errors.Select(e => e.Description))}");
                        }
                        else
                        {
                            Console.WriteLine($"Administrator '{userName}' has been successfully created and assigned to the '{AdminRoleName}' role.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occurred while seeding administrator: {ex.Message}");
                }
            })
                .GetAwaiter()
                .GetResult();

            return app;
        }


        /// <summary>
        /// Adds middleware to the application's request pipeline to track and monitor online users. 
        /// This extension method configures the <see cref="OnlineUsersMiddleware"/> to manage user activity 
        /// status, enabling features related to online user tracking.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance to configure.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> with the <see cref="OnlineUsersMiddleware"/> added.</returns>
        /// <remarks>
        /// This method simplifies the addition of <see cref="OnlineUsersMiddleware"/> to the pipeline, 
        /// which is responsible for identifying and tracking active users based on incoming requests.
        /// Ensure that <see cref="OnlineUsersMiddleware"/> is registered as part of the DI container 
        /// if it has any dependencies.
        /// </remarks>
        /// <example>
        /// <code>
        /// var app = builder.Build();
        /// app.EnableOnlineUsersCheck();
        /// </code>
        /// </example>

        public static IApplicationBuilder EnableOnlineUsersCheck(this IApplicationBuilder app)
        {
            return app.UseMiddleware<OnlineUsersMiddleware>();
        }

        /// <summary>
        /// Adds middleware to the application's request pipeline to populate and manage user-specific context data. 
        /// This extension method configures the <see cref="UserContextMiddleware"/> to provide additional 
        /// context about the user for each request, such as user ID and role-based information.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance to configure.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> with the <see cref="UserContextMiddleware"/> added.</returns>
        /// <remarks>
        /// This method simplifies the addition of <see cref="UserContextMiddleware"/> to the request pipeline, 
        /// enabling the extraction and storage of user-specific data, which can be accessed throughout the 
        /// application's lifecycle for the current request. Ensure that any dependencies required by 
        /// <see cref="UserContextMiddleware"/> are registered in the DI container.
        /// </remarks>
        /// <example>
        /// <code>
        /// var app = builder.Build();
        /// app.EnableUserContext();
        /// </code>
        /// </example>

        public static IApplicationBuilder EnableUserContext(this IApplicationBuilder app)
        {
            return app.UseMiddleware<UserContextMiddleware>();
        }

        /// <summary>
        /// Configures and registers recurring jobs with Hangfire for periodic execution.
        /// This method sets up a recurring job to check and update meal plan statuses daily 
        /// by calling the <see cref="IMealPlanService.UpdateMealPlansStatusAsync"/> method.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance used to configure the application pipeline.</param>
        /// <remarks>
        /// This extension method simplifies the registration of recurring Hangfire jobs within the application setup. 
        /// It registers a daily recurring job with a unique identifier, "checkMealplansStatus," to ensure the job 
        /// executes consistently. The job is enqueued by Hangfire on a daily schedule based on the specified Cron expression.
        /// Ensure that Hangfire is properly configured and that <see cref="IMealPlanService"/> is registered in 
        /// the dependency injection container.
        /// </remarks>
        /// <example>
        /// <code>
        /// var app = builder.Build();
        /// app.RegisterRecurringJobs();
        /// </code>
        /// </example>

        public static void RegisterRecurringJobs(this IApplicationBuilder app)
        {
            RecurringJob.AddOrUpdate<IMealPlanService>("checkMealplansStatus",
                service => service
                .UpdateMealPlansStatusAsync(CancellationToken.None),
                Cron.Daily);
        }
        
    }  
    
}
