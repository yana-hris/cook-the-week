namespace CookTheWeek.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.Measure;

    [Comment("The Measure of the Recipe Ingredient")]
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
