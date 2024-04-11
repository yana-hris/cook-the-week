namespace CookTheWeek.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;

    using static Common.EntityValidationConstants.Measure;

    [Comment("Measure of Recipe-Ingredient")]
    public class Measure
    {
        [Comment("Key Identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("Measure Name")]
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;
    }
}
