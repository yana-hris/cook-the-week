﻿namespace CookTheWeek.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System.Security.Claims;

    using Models;

    public class MealPlanEntityConfiguration : IEntityTypeConfiguration<MealPlan>
    {
        private readonly ClaimsPrincipal currentUser;

        public MealPlanEntityConfiguration(ClaimsPrincipal currentUser)
        {
            this.currentUser = currentUser;
        }
        public void Configure(EntityTypeBuilder<MealPlan> builder)
        {
            builder
                .Property(mp => mp.StartDate)
                .HasDefaultValueSql("GETDATE()");
           
            builder
                .HasOne(mp => mp.Owner)
                .WithMany(o => o.MealPlans)
                .HasForeignKey(mp => mp.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Property(mp => mp.IsFinished)
                .HasDefaultValue(false);

            builder
                .HasMany(mp => mp.Meals)
                .WithOne(m => m.MealPlan)
                .HasForeignKey(m => m.MealPlanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
