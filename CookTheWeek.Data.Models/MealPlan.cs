namespace CookTheWeek.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.EntityFrameworkCore;

    using static Common.EntityValidationConstants.MealPlan;

    [Comment("Meal Plan belongs to a user and consists of user-defined Meals")]
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

        [Comment("Is the Meal Plan already finished or not")]
        [Required]
        public bool IsFinished { get; set; }

        [Comment("A collection of the Meals, included in the Meal Plan")]
        [Required]
        public ICollection<Meal> Meals { get; set; }
    }
}
