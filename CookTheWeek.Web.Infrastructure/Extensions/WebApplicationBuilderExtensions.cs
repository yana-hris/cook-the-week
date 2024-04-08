namespace Microsoft.Extensions.DependencyInjection
{
    using System.Reflection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;

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
        public static void AddApplicationServices(this IServiceCollection services, Type serviceType)
        {
            Assembly? serviceAssembly = Assembly.GetAssembly(serviceType);

            if(serviceAssembly == null)
            {
                throw new InvalidOperationException("Invalid service type provided!");
            }
            Type[] implementationTypes = serviceAssembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("Service") && !t.IsInterface)
                .ToArray();

            foreach (Type implementationType in implementationTypes)
            {
                Type? interfaceType = implementationType.GetInterface($"I{implementationType.Name}");
                if(interfaceType == null)
                {
                    throw new InvalidOperationException(
                        $"No interface is provided for the service with name: {implementationType.Name}");
                }

                services.AddScoped(interfaceType, implementationType);
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
                if (await roleManager.RoleExistsAsync(AdminRoleName))
                {
                    return;
                }

                IdentityRole<Guid> role = new IdentityRole<Guid>(AdminRoleName);

                await roleManager.CreateAsync(role);

                ApplicationUser adminUser = await userManager.FindByNameAsync(userName);
                await userManager.AddToRoleAsync(adminUser, AdminRoleName);
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
