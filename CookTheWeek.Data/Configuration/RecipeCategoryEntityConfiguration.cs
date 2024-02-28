﻿namespace CookTheWeek.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models.RecipeEntities;
    public class RecipeCategoryEntityConfiguration : IEntityTypeConfiguration<RecipeCategory>
    {
        public void Configure(EntityTypeBuilder<RecipeCategory> builder)
        {
           
            builder
                .HasMany(rc => rc.Recipes)
                .WithOne(r => r.RecipeCategory)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder
                .HasData(GenerateRecipeCategories());
        }

        private ICollection<RecipeCategory> GenerateRecipeCategories()
        {
            return new HashSet<RecipeCategory>()
            {
                new RecipeCategory()
                {
                    Id = 1,
                    Name = "Breakfast"
                },
                new RecipeCategory()
                {
                    Id = 2,
                    Name = "Soup"
                },
                new RecipeCategory()
                {
                    Id = 3,
                    Name = "Salad"
                },
                new RecipeCategory()
                {
                    Id = 4,
                    Name = "Main Dish"
                },
                 new RecipeCategory()
                {
                    Id = 5,
                    Name = "Appetizer"
                },
                  new RecipeCategory()
                {
                    Id = 6,
                    Name = "Dessert"
                },
            };
        }
    }
}
