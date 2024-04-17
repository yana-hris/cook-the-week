
namespace CookTheWeek.WebApi
{
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Services.Data.Interfaces;

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

            // Add services to the container.
            builder.Services.AddApplicationServices(typeof(IIngredientService));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(setup =>
            {
                setup.AddPolicy("CookTheWeekDevelopmentPolicy", policyBuilder =>
                {
                    policyBuilder.WithOrigins("https://localhost:7170")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
                setup.AddPolicy("CookTheWeekProductionPolicy", policyBuilder =>
                {
                    policyBuilder.WithOrigins("http://cooktheweek.com")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            if (app.Environment.IsDevelopment())
            {
                app.UseCors("CookTheWeekDevelopmentPolicy");
            }
            else if(app.Environment.IsProduction())
            {
                app.UseCors("CookTheWeekProductionPolicy");
            }            

            app.Run();
        }
    }
}
