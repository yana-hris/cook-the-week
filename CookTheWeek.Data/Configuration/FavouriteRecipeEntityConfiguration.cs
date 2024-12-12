namespace CookTheWeek.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using CookTheWeek.Data.Models;

    public class FavouriteRecipeEntityConfiguration : IEntityTypeConfiguration<FavouriteRecipe>
    {
        public void Configure(EntityTypeBuilder<FavouriteRecipe> builder)
        {
            builder
                .HasKey(fr => new { fr.UserId, fr.RecipeId });

            builder
                .HasQueryFilter(fr => !fr.IsDeleted);

            builder
                .Property(r => r.IsDeleted)
                .HasDefaultValue(false);

            builder
                .HasOne(fr => fr.User)
                .WithMany(u => u.FavoriteRecipes)
                .HasForeignKey(fr => fr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(fr => fr.Recipe)
                .WithMany(r => r.FavouriteRecipes)
                .HasForeignKey(fr => fr.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);
                
        }
    }
}
