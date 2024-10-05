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
        public static void AddApplicationServicesOfType(this IServiceCollection services, Type[] assemblyTypes, string[] suffixes)
        {
            foreach (var assemblyType in assemblyTypes)
            {
                Assembly? assembly = Assembly.GetAssembly(assemblyType);

                if (assembly == null)
                {
                    throw new InvalidOperationException($"Invalid type provided for assembly: {assemblyType.Name}");
                }

                foreach (var suffix in suffixes)
                {
                    Type[] implementationTypes = assembly
                        .GetTypes()
                        .Where(t => t.Name.EndsWith(suffix) && !t.IsInterface && !t.IsAbstract)
                        .ToArray();

                    foreach (Type implementationType in implementationTypes)
                    {
                        Type[] interfaceTypes = implementationType
                            .GetInterfaces()
                            .Where(i => i.Name.StartsWith("I") && i.Name.Contains(suffix))
                            .ToArray();

                        if (!interfaceTypes.Any())
                        {
                            throw new InvalidOperationException(
                                $"No interface found for the type: {implementationType.Name}");
                        }

                        foreach (var interfaceType in interfaceTypes)
                        {
                            if (suffix.Contains("Category") && implementationType.IsGenericTypeDefinition)
                            {
                                services.AddScoped(interfaceType.GetGenericTypeDefinition(), implementationType.GetGenericTypeDefinition());
                            }
                            else
                            {
                                services.AddScoped(interfaceType, implementationType);
                            }
                        }
                    }
                }
            }
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
