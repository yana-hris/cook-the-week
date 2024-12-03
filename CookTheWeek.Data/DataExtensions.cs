namespace CookTheWeek.Data
{

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using static CookTheWeek.Data.SeedData.SeedData;
    public static class DataExtensions
    {

        /// <summary>
        /// Applies migrations and seeds the database.
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

                    logger.LogInformation("Applying database migrations..");
                    context.Database.Migrate();

                    if (seedData)
                    {
                        await SeedData(context);
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
        private static async Task SeedData(CookTheWeekDbContext context)
        {
            //context.Users.AddRange(SeedUsers());
            //context.RecipeCategories.AddRange(SeedRecipeCategories());
            //context.IngredientCategories.AddRange(SeedIngredientCategories());
            //context.Measures.AddRange(SeedMeasures());
            //context.Specifications.AddRange(SeedSpecifications());
            //context.Ingredients.AddRange(SeedIngredients());
            //context.Recipes.AddRange(SeedRecipes());
            //context.Steps.AddRange(SeedSteps());
            //context.RecipesIngredients.AddRange(SeedRecipeIngredients());
            //context.FavoriteRecipes.AddRange(SeedRecipeLikes());
            //context.MealPlans.AddRange(SeedMealPlans());
            //context.Meals.AddRange(SeedMeals());
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
