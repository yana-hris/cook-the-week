namespace CookTheWeek.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System.Reflection;

    using CookTheWeek.Data.Models;

    public class CookTheWeekDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        private bool seedDb;
        public CookTheWeekDbContext(DbContextOptions<CookTheWeekDbContext> options, bool seed = true)
            : base(options)
        {
            if (Database.IsRelational())
            {
                Database.Migrate();
            }
            else
            {
                Database.EnsureCreated();
            }

            this.seedDb = seed;
        }

        public DbSet<RecipeCategory> RecipeCategories { get; set; } = null!;
        public DbSet<IngredientCategory> IngredientCategories { get; set; } = null!;
        public DbSet<Measure> Measures { get; set; } = null!;
        public DbSet<Specification> Specifications { get; set; } = null!;
        public DbSet<Ingredient> Ingredients { get; set; } = null!;
        public DbSet<Recipe> Recipes { get; set; } = null!;
        public DbSet<Step> Steps { get; set; } = null!;
        public DbSet<RecipeIngredient> RecipesIngredients { get; set; } = null!;
        public DbSet<FavouriteRecipe> FavoriteRecipes { get; set; } = null!;
        public DbSet<Meal> Meals { get; set; } = null!;
        public DbSet<MealPlan> MealPlans { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder builder)
        {
            Assembly configAssembly = Assembly.GetAssembly(typeof(CookTheWeekDbContext)) ??
                Assembly.GetExecutingAssembly();
            
            builder.ApplyConfigurationsFromAssembly(configAssembly);

            if(seedDb)
            {
                var data = new SeedData.SeedData();

                builder.Entity<ApplicationUser>()
                    .HasData(data.SeedUsers());

                builder.Entity<RecipeCategory>()
                    .HasData(data.SeedRecipeCategories());

                builder.Entity<IngredientCategory>()
                    .HasData(data.SeedIngredientCategories());

                builder.Entity<Measure>()
                    .HasData(data.SeedMeasures());

                builder.Entity<Specification>()
                    .HasData(data.SeedSpecifications());

                builder.Entity<Ingredient>()
                    .HasData(data.SeedIngredients());

                builder.Entity<Recipe>()
                    .HasData(data.SeedRecipes());

                builder.Entity<Step>()
                   .HasData(data.SeedSteps());

                builder.Entity<RecipeIngredient>()
                    .HasData(data.SeedRecipeIngredients());
            }
            
            base.OnModelCreating(builder);
        }
    }
}
