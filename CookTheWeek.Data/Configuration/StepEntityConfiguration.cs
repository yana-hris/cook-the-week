namespace CookTheWeek.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class StepEntityConfiguration : IEntityTypeConfiguration<Step>
    {
        public void Configure(EntityTypeBuilder<Step> builder)
        {
            // Setting the relationship with Recipe and Cascade delete for Hard delete
            builder
                .HasOne(s => s.Recipe)
                .WithMany(r => r.Steps)
                .HasForeignKey(s => s.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Implementing the soft delete filtering
            builder
                .HasQueryFilter(s => !s.IsDeleted);

            builder
                .Property(s => s.IsDeleted)
                .HasDefaultValue(false);
            
        }
    }
}
