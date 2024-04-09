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
        }
    }
}
