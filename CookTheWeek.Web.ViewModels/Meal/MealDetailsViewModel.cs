namespace CookTheWeek.Web.ViewModels.Meal
{
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.Step;

    public class MealDetailsViewModel
    {
        public MealDetailsViewModel()
        {
            this.OilsHerbsSpicesSweeteners = new List<RecipeIngredientDetailsViewModel>();
            this.DiaryMeatSeafood = new List<RecipeIngredientDetailsViewModel>();
            this.Produce = new List<RecipeIngredientDetailsViewModel>();
            this.PastaGrainsBakery = new List<RecipeIngredientDetailsViewModel>();
            this.Legumes = new List<RecipeIngredientDetailsViewModel>();    
            this.NutsSeedsAndOthers = new List<RecipeIngredientDetailsViewModel>();
            this.CookingSteps = new List<StepViewModel>();
        }

        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public string? Description { get; set; }

        public int ServingSize { get; set; }

        public TimeSpan CookingTime { get; set; }

        public string CategoryName { get; set; } = null!;

        public ICollection<RecipeIngredientDetailsViewModel> OilsHerbsSpicesSweeteners { get; set; }
        public ICollection<RecipeIngredientDetailsViewModel> DiaryMeatSeafood { get; set; }
        public ICollection<RecipeIngredientDetailsViewModel> Produce { get; set; }
        public ICollection<RecipeIngredientDetailsViewModel> PastaGrainsBakery { get; set; }
        public ICollection<RecipeIngredientDetailsViewModel> Legumes { get; set; }
        public ICollection<RecipeIngredientDetailsViewModel> NutsSeedsAndOthers { get; set; }
        public List<StepViewModel> CookingSteps { get; set; }
    }
}
