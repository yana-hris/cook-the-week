namespace CookTheWeek.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidationConstants.Specification;

    [Comment("The Specification of the Recipe Ingredient")]
    public class Specification
    {
        [Comment("Specification Key Identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("Specification Description")]
        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;
    }
}
