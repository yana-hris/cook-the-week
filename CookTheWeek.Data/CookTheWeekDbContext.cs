﻿namespace CookTheWeek.Data
{
    using System.Reflection;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data.Models;

    public class CookTheWeekDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public CookTheWeekDbContext(DbContextOptions<CookTheWeekDbContext> options)
        : base(options)
        {
        }

        public DbSet<RecipeCategory> RecipeCategories { get; set; } = null!;
        public DbSet<IngredientCategory> IngredientCategories { get; set; } = null!;
        public DbSet<Measure> Measures { get; set; } = null!;
        public DbSet<Ingredient> Ingredients { get; set; } = null!;
        public DbSet<Recipe> Recipes { get; set; } = null!;
        public DbSet<Step> Steps { get; set; } = null!;
        public DbSet<RecipeIngredient> RecipesIngredients { get; set; } = null!;
        public DbSet<FavouriteRecipe> FavoriteRecipes { get; set; } = null!;
        public DbSet<Meal> Meals { get; set; } = null!;
        public DbSet<MealPlan> MealPlans { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<RecipeTag> RecipeTags { get; set; } = null!;
        public DbSet<RecipeRating> Ratings { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            Assembly configAssembly = Assembly.GetAssembly(typeof(CookTheWeekDbContext)) ??
                Assembly.GetExecutingAssembly();
            
            builder.ApplyConfigurationsFromAssembly(configAssembly);
            
            base.OnModelCreating(builder);
        }
    }
}
