namespace CookTheWeek.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.EntityFrameworkCore;

    using static Common.EntityValidationConstants.MealPlan;

    [Comment("Meal Plan")]
    public class MealPlan
    {
        public MealPlan()
        {
            Id = Guid.NewGuid();
            this.Meals = new HashSet<Meal>();
        }

        [Comment("Meal Plan Key Identifier")]
        [Key]
        public Guid Id { get; set; }

        [Comment("Meal Plan Name")]
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Comment("Meal Plan Owner Key Identifier")]
        [Required]
        [ForeignKey(nameof(Owner))]
        public Guid OwnerId { get; set; }
        public ApplicationUser Owner { get; set; } = null!;

        [Comment("Meal Plan Start Date")]
        [Required]
        public DateTime StartDate { get; set; }

        [Comment("Meal Plan Completion Identifier")]
        [Required]
        public bool IsFinished { get; set; } = false;

        [Comment("A collection of Meals, included in the Meal Plan")]
        [Required]
        public ICollection<Meal> Meals { get; set; }
    }
}
