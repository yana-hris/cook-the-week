namespace CookTheWeek.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class MealPlanEntityConfiguration : IEntityTypeConfiguration<MealPlan>
    {        
        
        public void Configure(EntityTypeBuilder<MealPlan> builder)
        {
            builder
                .Property(mp => mp.StartDate)
                .HasDefaultValueSql("GETUTCDATE()");

            builder
                .Property(mp => mp.IsFinished)
                .HasDefaultValue(false);
           
            builder
                .HasOne(mp => mp.Owner)
                .WithMany(o => o.MealPlans)
                .HasForeignKey(mp => mp.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder
                .HasMany(mp => mp.Meals)
                .WithOne(m => m.MealPlan)
                .HasForeignKey(m => m.MealPlanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
