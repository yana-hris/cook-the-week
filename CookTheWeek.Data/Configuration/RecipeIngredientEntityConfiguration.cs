
namespace CookTheWeek.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using CookTheWeek.Data.Models;
    public class RecipeIngredientEntityConfiguration : IEntityTypeConfiguration<RecipeIngredient>
    {
        public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
        {
            // Surrogate key
            builder.HasKey(ri => ri.Id);

            // Unique index where SpecificationId is not null
            builder.HasIndex(ri => new { ri.RecipeId, ri.IngredientId, ri.MeasureId, ri.SpecificationId })
                   .HasFilter("[SpecificationId] IS NOT NULL")
                   .IsUnique();

            // Unique index where SpecificationId is null
            builder.HasIndex(ri => new { ri.RecipeId, ri.IngredientId, ri.MeasureId })
                   .HasFilter("[SpecificationId] IS NULL")
                   .IsUnique();

            builder
                .Property(r => r.IsDeleted)
                .HasDefaultValue(false);

            builder
                .HasQueryFilter(ri => !ri.IsDeleted);


            builder
                .HasOne(ri => ri.Recipe)
                .WithMany(r => r.RecipesIngredients)
                .HasForeignKey(ri => ri.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);  

            // If an ingredient is deleted and there are existing recipeIngredients, the delete will be prevented
            builder
                .HasOne(ri => ri.Ingredient)
                .WithMany(i => i.RecipesIngredients)
                .HasForeignKey(ri => ri.IngredientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(ri => ri.Measure)
                .WithMany(m => m.RecipesIngredients)
                .HasForeignKey(ri => ri.MeasureId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(ri => ri.Specification)
                .WithMany(sp => sp.RecipesIngredients)
                .HasForeignKey(ri => ri.SpecificationId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .Property(ri => ri.Qty)
                .HasPrecision(18, 2);
        }
    }
}
