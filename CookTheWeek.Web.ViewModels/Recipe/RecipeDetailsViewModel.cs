﻿namespace CookTheWeek.Web.ViewModels.Recipe
{
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.Step;

    public class RecipeDetailsViewModel
    {
        public RecipeDetailsViewModel()
        {
            this.Steps = new List<StepViewModel>();
            this.DiaryMeatSeafood = new List<RecipeIngredientDetailsViewModel>();
            this.Produce = new List<RecipeIngredientDetailsViewModel>();
            this.Legumes = new List<RecipeIngredientDetailsViewModel>();
            this.PastaGrainsBakery = new List<RecipeIngredientDetailsViewModel>();
            this.OilsHerbsSpicesSweeteners = new List<RecipeIngredientDetailsViewModel>();
            this.NutsSeedsAndOthers = new List<RecipeIngredientDetailsViewModel>();
        }
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string? Description { get; set; } 


        public int Servings { get; set; }

        public TimeSpan TotalTime { get; set; } 

        public string ImageUrl { get; set; } = null!;

        public string CategoryName { get; set; }

        public string CreatedOn { get; set; } = null!;
        public List<StepViewModel> Steps { get; set; }
        public ICollection<RecipeIngredientDetailsViewModel> DiaryMeatSeafood { get; set; }
        public ICollection<RecipeIngredientDetailsViewModel> Produce { get; set; }
        public ICollection<RecipeIngredientDetailsViewModel> Legumes { get; set; }
        public ICollection<RecipeIngredientDetailsViewModel> PastaGrainsBakery { get; set; }
        public ICollection<RecipeIngredientDetailsViewModel> OilsHerbsSpicesSweeteners { get; set; }
        public ICollection<RecipeIngredientDetailsViewModel> NutsSeedsAndOthers { get; set; }
    }
}
