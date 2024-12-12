namespace CookTheWeek.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using CookTheWeek.Data.Models;

    public class RecipeRatingEntityConfiguration : IEntityTypeConfiguration<RecipeRating>
    {
        public void Configure(EntityTypeBuilder<RecipeRating> builder)
        {
            builder
                .HasKey(rr => new {rr.UserId, rr.RecipeId});

            builder
                .Property(rating => rating.IsDeleted)
                .HasDefaultValue(false);

            builder
                .HasQueryFilter(fr => !fr.IsDeleted);

            builder
                .Property(rating => rating.CreatedOn)
                .HasDefaultValueSql("GETUTCDATE()");

            builder
                .HasOne(rr => rr.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(rr => rr.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(rr => rr.Recipe)
                .WithMany(r => r.Ratings)
                .HasForeignKey(rr => rr.RecipeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
