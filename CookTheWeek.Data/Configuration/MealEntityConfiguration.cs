﻿namespace CookTheWeek.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class MealEntityConfiguration : IEntityTypeConfiguration<Meal>
    {
        public void Configure(EntityTypeBuilder<Meal> builder)
        {
            builder
                .Property(m => m.IsCooked)
                .HasDefaultValue(false);

            builder
                .Property(r => r.IsDeleted)
                .HasDefaultValue(false);

            builder
                .HasQueryFilter(m => !m.IsDeleted);

            builder
                .HasOne(m => m.Recipe)
                .WithMany(r => r.Meals)
                .HasForeignKey(m => m.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(m => m.MealPlan)
                .WithMany(mp => mp.Meals)
                .HasForeignKey(m => m.MealPlanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
