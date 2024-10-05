
namespace CookTheWeek.WebApi
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.Infrastructure.Extensions;
    using CookTheWeek.Services.Data.Services;
    using CookTheWeek.Web.Infrastructure.Middlewares;

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

            builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.SignIn.RequireConfirmedAccount = builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");
                options.Password.RequireLowercase = builder.Configuration.GetValue<bool>("Password:RequireLowercase");
                options.Password.RequireUppercase = builder.Configuration.GetValue<bool>("Password:RequireUppercase");
                options.Password.RequireNonAlphanumeric = builder.Configuration.GetValue<bool>("Password:RequireNonAlphanumeric");
                options.Password.RequireDigit = builder.Configuration.GetValue<bool>("Password:RequireDigit");
                options.Password.RequiredLength = builder.Configuration.GetValue<int>("Password:RequiredLength");
            })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<CookTheWeekDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddScoped<IUserContext, UserContext>();

            builder.Services.AddScoped<IFavouriteRecipeRepository, FavouriteRecipeRepository>();
            builder.Services.AddScoped<IFavouriteRecipeService, FavouriteRecipeService>();
            builder.Services.AddScoped<IMealplanRepository, MealplanRepository>();
            builder.Services.AddScoped<IMealPlanService, MealPlanService>();
            builder.Services.AddScoped<IValidationService, ValidationService>();
            builder.Services.AddScoped<IMealService, MealService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IIngredientService, IngredientService>();
            builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
            builder.Services.AddScoped<IValidationService, ValidationService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowDevelopment", policy =>
                {
                    policy.WithOrigins("https://localhost:7170")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });

                options.AddPolicy("AllowProduction", policy =>
                {
                    policy.WithOrigins("http://cooktheweek.com")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Developer-specific middlewares first
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseCors("AllowDevelopment");  
            }
            else
            {
                app.UseHsts();  // Apply HSTS before HTTPS redirection
                app.UseCors("AllowProduction");  // Apply CORS early
            }

            app.UseHttpsRedirection();  // Force HTTPS early

            // Custom middleware for user context
            app.UseMiddleware<UserContextMiddleware>();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();  

            app.Run();
        }
    }
}
