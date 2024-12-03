namespace CookTheWeek.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class RecipeTagEntityConfiguration : IEntityTypeConfiguration<RecipeTag>
    {
        public void Configure(EntityTypeBuilder<RecipeTag> builder)
        {
            builder
                .HasKey(rt => new {rt.RecipeId, rt.TagId});

            builder
                .HasOne(rt => rt.Recipe)
                .WithMany(r => r.RecipeTags)
                .HasForeignKey(rt => rt.RecipeId)
                .OnDelete(DeleteBehavior.Cascade); // Delete RecipeTags when a Recipe is deleted

            builder
                .HasOne(rt => rt.Tag)
                .WithMany(t => t.RecipeTags)
                .HasForeignKey(rt => rt.TagId)
                .OnDelete(DeleteBehavior.Cascade); // Delete RecipeTags when a Tag is deleted
        }
    }
}
