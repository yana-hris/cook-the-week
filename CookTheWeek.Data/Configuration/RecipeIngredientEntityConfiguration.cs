
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

           // Recipe is set to soft delete, so recipeIngredients will not be actually deleted
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

            builder
                .HasData(GenerateRecipeIngredients());
        }

        private ICollection<RecipeIngredient> GenerateRecipeIngredients()
        {
            ICollection<RecipeIngredient> recipeIngredients = new HashSet<RecipeIngredient>()
            {
                 new RecipeIngredient()
                 {
                    RecipeId = Guid.Parse("377A3703-E719-42DB-B013-19E5BD23892A"),
                    IngredientId = 11,
                    Qty = 500,
                    MeasureId = 5
                 },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("377A3703-E719-42DB-B013-19E5BD23892A"),
                    IngredientId = 113,
                    Qty = 2,
                    MeasureId = 1
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("377A3703-E719-42DB-B013-19E5BD23892A"),
                    IngredientId = 118,
                    Qty = 1,
                    MeasureId = 6
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("377A3703-E719-42DB-B013-19E5BD23892A"),
                    IngredientId = 122,
                    Qty = 250,
                    MeasureId = 5
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("377A3703-E719-42DB-B013-19E5BD23892A"),
                    IngredientId = 154,
                    Qty = 3,
                    MeasureId = 8
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("377A3703-E719-42DB-B013-19E5BD23892A"),
                    IngredientId = 54,
                    Qty = 1,
                    MeasureId = 8
                }
            };

            return recipeIngredients;
        }
    }
}
