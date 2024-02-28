namespace CookTheWeek.Data
{
    using System.Reflection;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using Models;
    using Models.IgredientEntities;
    using Models.RecipeEntities;

    public class CookTheWeekDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public CookTheWeekDbContext(DbContextOptions<CookTheWeekDbContext> options)
            : base(options)
        {
        }

        public DbSet<IngredientCategory> IngredientCategories { get; set; } = null!;

        public DbSet<Ingredient> Ingredients { get; set; } = null!;

        public DbSet<RecipeCategory> RecipeCategories { get; set; } = null!;

        public DbSet<Recipe> Recipes { get; set; } = null!;

        public DbSet<Measure> Measures { get; set; } = null!;

        public DbSet<RecipeIngredient> RecipesIngredients { get; set; } = null!;

        public DbSet<Specification> Specifications { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            Assembly configAssembly = Assembly.GetAssembly(typeof(CookTheWeekDbContext)) ??
                Assembly.GetExecutingAssembly();
            
            builder.ApplyConfigurationsFromAssembly(configAssembly);
            
            base.OnModelCreating(builder);
        }
    }
}
