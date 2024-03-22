﻿namespace CookTheWeek.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static CookTheWeek.Common.EntityValidationConstants.IngredientCategory;
    public class IngredientCategory
    {
        public IngredientCategory()
        {
            Ingredients = new HashSet<Ingredient>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public ICollection<Ingredient> Ingredients { get; set; }
    }
}