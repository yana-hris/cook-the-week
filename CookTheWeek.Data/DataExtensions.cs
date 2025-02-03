namespace CookTheWeek.Data
{

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using static CookTheWeek.Data.SeedData.SeedData;
    public static class DataExtensions
    {

        /// <summary>
        /// Applies migrations and seeds the database upon application start up. Meant for use in Development Env. only
        /// </summary>
        /// <param name="services">The service provider containing the DbContext.</param>
        public static async Task ApplyMigrationsAndSeedData(this IServiceProvider services, bool seedData = false)
        {

            using var scope = services.CreateScope();
            var provider = scope.ServiceProvider;

            var logger = provider.GetRequiredService<ILogger<CookTheWeekDbContext>>();
            var configuration = provider.GetRequiredService<IConfiguration>();

            try
            {
                var context = provider.GetRequiredService<CookTheWeekDbContext>();

                if (!context.Database.CanConnect())
                {
                    logger.LogInformation("Database does not exist. Creating now...");
                    await context.Database.EnsureCreatedAsync(); // 🔹 Ensures the DB is created
                    logger.LogInformation("Database created successfully.");
                }

                logger.LogInformation("Applying migrations...");
                await context.Database.MigrateAsync();
                logger.LogInformation("Migrations applied successfully.");
                await context.SaveChangesAsync();

                await using var transaction = await context.Database.BeginTransactionAsync();
                
                   
                
                if (seedData)
                {
                    logger.LogInformation("Seeding data...");
                   
                    bool dataSeeded = await SeedData(context, configuration);
                    

                    if (dataSeeded)
                    {
                        logger.LogInformation("Database seeding completed successfully.");
                    }
                    else
                    {
                        logger.LogInformation("Database already seeded. Skipping.");
                    }
                }
                
                await transaction.CommitAsync(); // ✅ Commit all changes if everything succeeded
                logger.LogInformation("Database migration and seeding completed successfully.");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred while migrating or seeding the database.Error info: {ex.Message}. StackTrace: {ex.StackTrace}");
                throw;
            }
            
        }

        /// <summary>
        /// Helper method for programatically seeding new data into the database. To add new data pass it to any of the methods commented below
        /// </summary>
        /// <param name="context">The app DB Context</param>
        private static async Task<bool> SeedData(CookTheWeekDbContext context, IConfiguration configuration)
        {
            Initialize(configuration);

            if (context.Users.Any() &&
                 context.RecipeCategories.Any() &&
                 context.IngredientCategories.Any() &&
                 context.Measures.Any() &&
                 context.Ingredients.Any() &&
                 context.Recipes.Any() &&
                 context.Steps.Any() &&
                 context.RecipesIngredients.Any() &&
                 context.FavoriteRecipes.Any() &&
                 context.MealPlans.Any() &&
                 context.Meals.Any() &&
                 context.Tags.Any() &&
                 context.RecipeTags.Any())
            {
                return false; // Database is already seeded
            }

            try
            {
                if (!context.Users.Any())
                {
                    context.Users.AddRange(SeedUsers());
                }
                if (!context.RecipeCategories.Any())
                {
                    await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT RecipeCategories ON");                    
                    context.RecipeCategories.AddRange(SeedRecipeCategories());
                    await context.SaveChangesAsync();
                    await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT RecipeCategories OFF");
                }
                if (!context.IngredientCategories.Any())
                {
                    await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT IngredientCategories ON");
                    context.IngredientCategories.AddRange(SeedIngredientCategories());
                    await context.SaveChangesAsync();
                    await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT IngredientCategories OFF");
                }
                if (!context.Measures.Any())
                {
                    await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Measures ON");
                    context.Measures.AddRange(SeedMeasures());
                    await context.SaveChangesAsync();
                    await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Measures OFF");
                }
                if (!context.Ingredients.Any())
                {
                    await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Ingredients ON");
                    context.Ingredients.AddRange(SeedIngredients());
                    await context.SaveChangesAsync();
                    await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Ingredients OFF");
                }
                if (!context.Recipes.Any())
                {
                    context.Recipes.AddRange(SeedRecipes());
                }
                if (!context.Steps.Any())
                {
                    context.Steps.AddRange(SeedSteps());
                }
                if (!context.RecipesIngredients.Any())
                {
                    await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT RecipesIngredients ON");
                    context.RecipesIngredients.AddRange(SeedRecipeIngredients());
                    await context.SaveChangesAsync();
                    await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT RecipesIngredients OFF");
                }
                if (!context.FavoriteRecipes.Any())
                {
                    context.FavoriteRecipes.AddRange(SeedRecipeLikes());
                }
                if (!context.MealPlans.Any())
                {
                    context.MealPlans.AddRange(SeedMealPlans());
                }
                if (!context.Meals.Any())
                {
                    context.Meals.AddRange(SeedMeals());
                }
                if (!context.Tags.Any())
                {
                    await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Tags ON");
                    context.Tags.AddRange(SeedTags());
                    await context.SaveChangesAsync();
                    await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Tags OFF");
                }
                if (!context.RecipeTags.Any())
                {
                    context.RecipeTags.AddRange(SeedRecipeTags());
                }
               
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while seeding the database.", ex);
            }
        }

        
    }
}
