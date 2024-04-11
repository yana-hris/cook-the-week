namespace CookTheWeek.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;

    using static Common.EntityValidationConstants.Specification;

    [Comment("Specification of Recipe-Ingredient")]
    public class Specification
    {
        [Comment("Key Identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("Specification Description")]
        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;
    }
}
