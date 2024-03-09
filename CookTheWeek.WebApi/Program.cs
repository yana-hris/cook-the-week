
namespace CookTheWeek.WebApi
{
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Web.Infrastructure.Extensions;
    using Services.Interfaces;

    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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

            app.Run();
        }
    }
}
