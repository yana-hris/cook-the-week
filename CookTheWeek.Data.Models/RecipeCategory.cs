﻿namespace CookTheWeek.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using CookTheWeek.Data.Models.Interfaces;
    using Microsoft.EntityFrameworkCore;

    using static Common.EntityValidationConstants.CategoryValidation;

    [Comment("Recipes Category")]
    public class RecipeCategory : ICategory
    {
        public RecipeCategory()
        {
            Recipes = new HashSet<Recipe>();
        }

        [Comment("Key Identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("Recipe Category Name")]
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Comment("A collection in Recipes in Category")]
        public ICollection<Recipe> Recipes { get; set; }
    }
}
