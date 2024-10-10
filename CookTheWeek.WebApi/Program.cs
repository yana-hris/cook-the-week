
namespace CookTheWeek.WebApi
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;

    using CookTheWeek.Services.Data.Events.Dispatchers;
    using CookTheWeek.Services.Data.Events.EventHandlers;
    using CookTheWeek.Services.Data.Helpers;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Services.Data.Services;
    using CookTheWeek.Web.Infrastructure.Extensions;
    using CookTheWeek.Web.Infrastructure.Middlewares;
    using Microsoft.AspNetCore.Authentication.Cookies;

    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add logging configuration
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            string? connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
            builder.Services.AddDbContext<CookTheWeekDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Add Identity and cookie authentication
            builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<CookTheWeekDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddScoped<IUserContext, UserContext>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IIngredientAggregatorHelper, IngredientAggregatorHelper>();
            builder.Services.AddScoped<IRecipeSoftDeletedEventHandler, RecipeSoftDeletedEventHandler>();
            builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

            // Add Application Services
            builder.Services.AddScoped<ICategoryRepository<RecipeCategory>, CategoryRepository<RecipeCategory>>();
            builder.Services.AddScoped<ICategoryRepository<IngredientCategory>, CategoryRepository<IngredientCategory>>();

            var suffixes = new[] { "Repository", "Service", "Factory" };
            var assemblyTypes = new[] { typeof(RecipeRepository).Assembly,
                        typeof(CategoryRepository<>).Assembly,
                        typeof(RecipeService).Assembly };

            builder.Services.AddServicesByConvention(
                assemblyTypes,
                suffixes);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

           
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowDevelopment", policy =>
                {
                    policy.WithOrigins("https://localhost:7171")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                    policy.WithOrigins("https://localhost:44311")
                         .AllowAnyHeader()
                         .AllowAnyMethod();
                });

                //options.AddPolicy("AllowProduction", policy =>
                //{
                //    policy.WithOrigins("http://cooktheweek.com")
                //          .AllowAnyHeader()
                //          .AllowAnyMethod();
                //});
            });

            var app = builder.Build();

            // Developer-specific middlewares first
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
                app.UseCors("AllowDevelopment");  
            }
            else
            {
                app.UseHsts();  // Apply HSTS before HTTPS redirection
                //app.UseCors("AllowProduction");  // Apply CORS early
            }

            app.UseHttpsRedirection();  // Force HTTPS early

            // Custom middleware for user context
            app.UseRouting();

            app.UseAuthentication();
            //app.UseMiddleware<UserContextMiddleware>();
            app.UseAuthorization();

            app.MapControllers();  

            app.Run();
        }
    }
}
