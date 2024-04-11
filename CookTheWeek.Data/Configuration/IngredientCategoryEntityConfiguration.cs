namespace CookTheWeek.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;
    public class IngredientCategoryEntityConfiguration : IEntityTypeConfiguration<IngredientCategory>
    {
        public void Configure(EntityTypeBuilder<IngredientCategory> builder)
        {
            // If ingredients in the deleted category exist, delete will be prevented
            builder
                .HasMany(ic => ic.Ingredients)
                .WithOne(i => i.Category)
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
