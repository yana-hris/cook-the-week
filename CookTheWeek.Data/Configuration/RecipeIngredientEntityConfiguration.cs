
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
                .Property(ri => ri.Qty)
                .HasPrecision(18, 2);

            builder
                .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

            builder
                .HasData(GenerateRecipeIngredients());
        }

        private ICollection<RecipeIngredient> GenerateRecipeIngredients()
        {
            ICollection<RecipeIngredient> recipeIngredients = new HashSet<RecipeIngredient>()
            {
                 new RecipeIngredient()
                 {
                    RecipeId = Guid.Parse("1bec581c-d30c-4adf-ae77-7e7401b37b04"),
                    IngredientId = 11,
                    Qty = 500,
                    MeasureId = 5
                 },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("1bec581c-d30c-4adf-ae77-7e7401b37b04"),
                    IngredientId = 113,
                    Qty = 2,
                    MeasureId = 1
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("1bec581c-d30c-4adf-ae77-7e7401b37b04"),
                    IngredientId = 118,
                    Qty = 1,
                    MeasureId = 6
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("1bec581c-d30c-4adf-ae77-7e7401b37b04"),
                    IngredientId = 122,
                    Qty = 250,
                    MeasureId = 5
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("1bec581c-d30c-4adf-ae77-7e7401b37b04"),
                    IngredientId = 154,
                    Qty = 3,
                    MeasureId = 8
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("1bec581c-d30c-4adf-ae77-7e7401b37b04"),
                    IngredientId = 54,
                    Qty = 1,
                    MeasureId = 8
                }
            };

            return recipeIngredients;
        }
    }
}
