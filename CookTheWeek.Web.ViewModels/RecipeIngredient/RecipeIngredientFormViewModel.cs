﻿namespace CookTheWeek.Web.ViewModels.RecipeIngredient
{
    using System.ComponentModel.DataAnnotations;

    public class RecipeIngredientFormViewModel
    {
        [Required]
        public int IngredientId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Qty { get; set; }

        [Required]
        public int MeasureId { get; set; }

        public int? SpecificationId { get; set; }

        public ICollection<RecipeIngredientSelectMeasureViewModel>? Measures { get; set; }

        public ICollection<RecipeIngredientSelectSpecificationViewModel>? Specifications { get; set; }
    }
}
