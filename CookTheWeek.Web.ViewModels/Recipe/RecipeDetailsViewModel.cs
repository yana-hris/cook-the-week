namespace CookTheWeek.Web.ViewModels.Recipe
{
    using CookTheWeek.Web.ViewModels.RecipeIngredient;

    public class RecipeDetailsViewModel
    {
        public RecipeDetailsViewModel()
        {
            this.MainIngredients = new List<RecipeIngredientDetailsViewModel>();
            this.SecondaryIngredients = new List<RecipeIngredientDetailsViewModel>();
            this.AdditionalIngredients = new List<RecipeIngredientDetailsViewModel>();
        }
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string? Description { get; set; } 

        public string Instructions { get; set; } = null!;

        public int Servings { get; set; }

        public string TotalTime { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public string CategoryName { get; set; }

        public string CreatedOn { get; set; } = null!;

        public ICollection<RecipeIngredientDetailsViewModel> MainIngredients { get; set; }
        public ICollection<RecipeIngredientDetailsViewModel> SecondaryIngredients { get; set; }
        public ICollection<RecipeIngredientDetailsViewModel> AdditionalIngredients { get; set; }
    }
}
