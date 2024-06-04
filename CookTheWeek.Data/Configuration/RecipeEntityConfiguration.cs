namespace CookTheWeek.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class RecipeEntityConfiguration : IEntityTypeConfiguration<Recipe>
    {        
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            // Applying global filter to exclude all soft-deleted recipes from all queries
            builder
                .HasQueryFilter(r => !r.IsDeleted);
            
            builder
               .Property(r => r.CreatedOn)
               .HasDefaultValueSql("GETDATE()");
           
            builder
                .Property(r => r.IsDeleted)
                .HasDefaultValue(false);

            builder
                .HasOne(r => r.Category)
                .WithMany(rc => rc.Recipes)
                .HasForeignKey(r => r.CategoryId);

            builder
                .HasOne(r => r.Owner)
                .WithMany(o => o.Recipes)
                .HasForeignKey(r => r.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
            

            builder
                .HasMany(r => r.RecipesIngredients)
                .WithOne(ri => ri.Recipe)
                .HasForeignKey(ri => ri.RecipeId);

            builder
                .HasMany(r => r.Steps)
                .WithOne(s => s.Recipe)
                .HasForeignKey(s => s.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(r => r.Meals)
                .WithOne(m => m.Recipe)
                .HasForeignKey(m => m.RecipeId);

            builder
                .HasMany(r => r.FavouriteRecipes)
                .WithOne(fr => fr.Recipe)
                .HasForeignKey(fr =>fr.RecipeId);
            
            
        }
    }
}
