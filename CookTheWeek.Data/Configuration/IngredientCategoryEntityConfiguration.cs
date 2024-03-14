namespace CookTheWeek.Data.Configuration
{
    using CookTheWeek.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    public class IngredientCategoryEntityConfiguration : IEntityTypeConfiguration<IngredientCategory>
    {
        public void Configure(EntityTypeBuilder<IngredientCategory> builder)
        {
            builder
                .HasMany(ic => ic.Ingredients)
                .WithOne(i => i.IngredientCategory)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasData(GenerateIngredientCategories());
        }

        private ICollection<IngredientCategory> GenerateIngredientCategories()
        {
            return new HashSet<IngredientCategory>()
            {
                new IngredientCategory()
                {
                    Id = 1,
                    Name = "Eggs, Milk and Diary products"
                },
                new IngredientCategory()
                {
                    Id = 2,
                    Name = "Meat, Ground Meat and Sausage"
                },
                new IngredientCategory()
                {
                    Id = 3,
                    Name = "Beans, Lentils and Legumes"
                },
                new IngredientCategory()
                {
                    Id = 4,
                    Name = "Flour and Baking Products"
                },
                new IngredientCategory()
                {
                    Id = 5,
                    Name = "Herbs and Spices"
                },
                new IngredientCategory()
                {
                    Id = 6,
                    Name = "Sweeteners"
                },
                new IngredientCategory()
                {
                    Id = 7,
                    Name = "Pasta and Grains"
                },
                new IngredientCategory()
                {
                    Id = 8,
                    Name = "Vegetables"
                },
                new IngredientCategory()
                {
                    Id = 9,
                    Name = "Fruits"
                },
                new IngredientCategory()
                {
                    Id = 10,
                    Name = "Fats and Oils"
                },
                new IngredientCategory()
                {
                    Id = 11,
                    Name = "Fish and Seafood"
                },
                 new IngredientCategory()
                {
                    Id = 12,
                    Name = "Others"
                },
                 new IngredientCategory()
                {
                    Id = 13,
                    Name = "Nuts and seeds"
                },

            };
        }
    }
}
