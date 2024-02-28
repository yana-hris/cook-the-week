namespace CookTheWeek.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Data.Models;

    public class MeasureEntityConfiguration : IEntityTypeConfiguration<Measure>
    {
        public void Configure(EntityTypeBuilder<Measure> builder)
        {
            builder
                .HasData(GenerateMeasures());
        }

        private ICollection<Measure> GenerateMeasures()
        {
            return new HashSet<Measure>()
            {
                new Measure()
                {
                    Id = 1,
                    Name = "piece(s)"
                },
                 new Measure()
                {
                    Id = 2,
                    Name = "clove(s)"
                },
                  new Measure()
                {
                    Id = 3,
                    Name = "ml"
                },
                   new Measure()
                {
                    Id = 4,
                    Name = "l"
                },
                    new Measure()
                {
                    Id = 5,
                    Name = "g"
                },
                     new Measure()
                {
                    Id = 6,
                    Name = "kg"
                },
                      new Measure()
                {
                    Id = 7,
                    Name = "tsp"
                },
                       new Measure()
                {
                    Id = 8,
                    Name = "tbsp"
                },
                        new Measure()
                {
                    Id = 9,
                    Name = "cup"
                },
            };
        }
    }
}
