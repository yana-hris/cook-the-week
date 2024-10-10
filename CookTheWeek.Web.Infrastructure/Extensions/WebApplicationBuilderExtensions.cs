namespace CookTheWeek.Web.Infrastructure.Extensions
{
    using System.Reflection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    using CookTheWeek.Data.Models;
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
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembliesToScan"></param>
        /// <param name="suffixes"></param>
        /// <param name="interfaceTypes"></param>
        /// <returns></returns>
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
                            // log error $"Failed to create role: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}"
                            return;
                        }

                    }

                    ApplicationUser? adminUser = await userManager.FindByNameAsync(userName);

                    if (adminUser == null)
                    {
                        //log error $"Admin user '{userName}' not found."
                        return;
                    }

                    var result = await userManager.AddToRoleAsync(adminUser, AdminRoleName);
                    if (!result.Succeeded)
                    {
                        //log error $"Failed to assign role to user: {string.Join(", ", result.Errors.Select(e => e.Description))}"

                    }
                }
                catch (Exception ex)
                {
                    // log error (ex, "Error occurred while seeding administrator.");
                }


            })
                .GetAwaiter()
                .GetResult();

            return app;
        }

        public static IApplicationBuilder EnableOnlineUsersCheck(this IApplicationBuilder app)
        {
            return app.UseMiddleware<OnlineUsersMiddleware>();
        }

        
    }  
    
}
