
namespace CookTheWeek.WebApi
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Services.Data.Services;
    using CookTheWeek.Web.Infrastructure.Middlewares;
    using CookTheWeek.Web.ViewModels.Admin.CategoryAdmin;
    using CookTheWeek.Web.ViewModels.Category;
    using CookTheWeek.Services.Data.Events.Dispatchers;
    using CookTheWeek.Services.Data.Events.EventHandlers;

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


            builder.Services.AddScoped<IRecipeSoftDeletedEventHandler, RecipeSoftDeletedEventHandler>();
            builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

            builder.Services.AddScoped<IUserContext, UserContext>();

            builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();  
            builder.Services.AddScoped<IRecipeService, RecipeService>();

            builder.Services.AddScoped<IStepRepository, StepRepository>();
            builder.Services.AddScoped<IStepService, StepService>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<IMealRepository, MealRepository>();
            builder.Services.AddScoped<IMealService, MealService>();

            builder.Services.AddScoped<IFavouriteRecipeRepository, FavouriteRecipeRepository>();
            builder.Services.AddScoped<IFavouriteRecipeService, FavouriteRecipeService>();

            builder.Services.AddScoped<IRecipeIngredientRepository, RecipeIngredientRepository>();
            builder.Services.AddScoped<IRecipeIngredientService, RecipeIngredientService>();

            builder.Services.AddScoped<IMealplanRepository, MealplanRepository>();
            builder.Services.AddScoped<IMealPlanService, MealPlanService>();

            builder.Services.AddScoped<ICategoryRepository<RecipeCategory>, CategoryRepository<RecipeCategory>>();
            builder.Services.AddScoped<ICategoryRepository<IngredientCategory>, CategoryRepository<IngredientCategory>>();
            builder.Services.AddScoped<ICategoryService<
                                                        RecipeCategory,
                                                        RecipeCategoryAddFormModel,
                                                        RecipeCategoryEditFormModel,
                                                        RecipeCategorySelectViewModel>, RecipeCategoryService>();
            builder.Services.AddScoped<ICategoryService<
                                                        IngredientCategory,
                                                        IngredientCategoryAddFormModel,
                                                        IngredientCategoryEditFormModel,
                                                        IngredientCategorySelectViewModel>, IngredientCategoryService>();
            builder.Services.AddScoped<IValidationService, ValidationService>();

            builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
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
