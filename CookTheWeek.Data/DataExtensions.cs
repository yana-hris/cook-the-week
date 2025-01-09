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
            using (var scope = services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var logger = provider.GetRequiredService<ILogger<CookTheWeekDbContext>>();
                

                try
                {
                    var context = provider.GetRequiredService<CookTheWeekDbContext>();
                    var configuration = provider.GetRequiredService<IConfiguration>();

                    logger.LogInformation("Applying database migrations..");
                    context.Database.Migrate();

                    if (seedData)
                    {
                        await SeedData(context, configuration);
                    }

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                    throw;
                }
            }
        }

        /// <summary>
        /// Helper method for programatically seeding new data into the database. To add new data pass it to any of the methods commented below
        /// </summary>
        /// <param name="context">The app DB Context</param>
        private static async Task SeedData(CookTheWeekDbContext context, IConfiguration configuration)
        {

            if (!context.Users.Any())
            {
                context.Users.AddRange(SeedUsers(configuration));
            }
            if (!context.RecipeCategories.Any())
            {
                context.RecipeCategories.AddRange(SeedRecipeCategories());
            }
            if (!context.IngredientCategories.Any())
            {
                context.IngredientCategories.AddRange(SeedIngredientCategories());
            }
            if (!context.Measures.Any())
            {
                context.Measures.AddRange(SeedMeasures());
            }
            if (!context.Ingredients.Any())
            {
                context.Ingredients.AddRange(SeedIngredients());
            }
            if (!context.Recipes.Any())
            {
                context.Recipes.AddRange(SeedRecipes(configuration));
            }
            if (!context.Steps.Any())
            {
                context.Steps.AddRange(SeedSteps());
            }
            if (!context.RecipesIngredients.Any())
            {
                context.RecipesIngredients.AddRange(SeedRecipeIngredients());
            }
            if (!context.FavoriteRecipes.Any())
            {
                context.FavoriteRecipes.AddRange(SeedRecipeLikes(configuration));
            }
            if (!context.MealPlans.Any())
            {
                context.MealPlans.AddRange(SeedMealPlans(configuration));
            }
            if (!context.Meals.Any())
            {
                context.Meals.AddRange(SeedMeals());
            }
            if (!context.Tags.Any())
            {                
                // Add tags with explicit IDs
                context.Tags.AddRange(SeedTags());
            }
            if (!context.RecipeTags.Any())
            {
                context.RecipeTags.AddRange(SeedRecipeTags());
            }
            // Continue or change...

            await context.SaveChangesAsync();
        }


    }
}
