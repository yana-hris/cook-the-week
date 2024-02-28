namespace CookTheWeek.Data.Configuration
{
    using CookTheWeek.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    public class SpecificationEntityConfiguration : IEntityTypeConfiguration<Specification>
    {
        public void Configure(EntityTypeBuilder<Specification> builder)
        {
            builder
                .HasData(GenerateSpecifications());
        }

        private ICollection<Specification> GenerateSpecifications()
        {
            return new HashSet<Specification>()
            {
                new Specification
                {
                    Id = 1,
                    Description = "frozen"
                },
                new Specification
                {
                    Id = 2,
                    Description = "canned"
                },
                new Specification
                {
                    Id = 3,
                    Description = "sliced"
                },
                new Specification
                {
                    Id = 4,
                    Description = "pre-cooked"
                },
                new Specification
                {
                    Id = 5,
                    Description = "grated"
                },
                new Specification
                {
                    Id = 6,
                    Description = "fermented"
                },
                new Specification
                {
                    Id = 7,
                    Description = "blended"
                },
                new Specification
                {
                    Id = 8,
                    Description = "finely-chopped"
                },
                new Specification
                {
                    Id = 9,
                    Description = "fresh"
                }
            };
        }
    }
}
