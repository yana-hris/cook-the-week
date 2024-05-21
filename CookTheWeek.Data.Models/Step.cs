namespace CookTheWeek.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;

    [Comment("Cooking Step")]
    public class Step
    {
        [Comment("Cooking Step Instructions")]
        [Required]
        public string Description { get; set; } = null!;
    }
}
