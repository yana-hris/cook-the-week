
namespace CookTheWeek.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using CookTheWeek.Data.Models;
    public class RecipeIngredientEntityConfiguration : IEntityTypeConfiguration<RecipeIngredient>
    {
        public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
        {

            builder
                .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

           // Recipe is set to soft delete, but recipe ingredients are hard-deleted
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
                .Property(ri => ri.Qty)
                .HasPrecision(18, 2);
        }
    }
}
