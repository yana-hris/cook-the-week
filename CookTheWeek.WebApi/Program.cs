
namespace CookTheWeek.WebApi
{
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Services.Data.Interfaces;
    using Web.Infrastructure.Extensions;
    using CookTheWeek.Services.Data;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add logging configuration
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            string? connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
            builder.Services.AddDbContext<CookTheWeekDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Add services to the container.
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRecipeService, RecipeService>();
            builder.Services.AddScoped<IFavouriteRecipeService, FavouriteRecipeService>();
            builder.Services.AddScoped<IIngredientService, IngredientService>();

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

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseCors("AllowDevelopment");
            }
            else
            {
                app.UseHsts();
                app.UseCors("AllowProduction");
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
