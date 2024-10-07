namespace CookTheWeek.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.EntityFrameworkCore;

    using static CookTheWeek.Common.EntityValidationConstants.StepValidation;

    [Comment("Cooking Step")]
    public class Step
    {
        [Comment("Step Key Identifier")]
        [Key]
        public int Id { get; set; }


        [Comment("Recipe Key Identifier")]
        [Required]
        [ForeignKey(nameof(Recipe))]
        public Guid RecipeId { get; set; }
        public Recipe Recipe { get; set; } = null!;


        [Comment("Cooking Step Instructions")]
        [Required]
        [MaxLength(StepDescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Comment("Soft Delete the Step upon Recipe Deletion")]
        [Required]
        public bool IsDeleted { get; set; }

    }
}
