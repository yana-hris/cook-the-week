﻿namespace CookTheWeek.Web.ViewModels.Recipe
{
   
    public class RecipeAllViewModel
    {
        public Guid Id { get; set; } 
        public Guid OwnerId { get; set; }
        public bool IsSiteRecipe { get; set; }
        public bool IsIncludedInActiveMealPlan { get; set; } = false;
        public string ImageUrl { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string CookingTime { get; set; } = null!;
        public string MealType { get; set; } = null!;
        public int Servings { get; set; }

    }
}
