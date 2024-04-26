namespace CookTheWeek.Services.Data.Models.Ingredient
{
    using CookTheWeek.Web.ViewModels.Admin.IngredientAdmin;

    public class AllIngredientsFilteredAndPagedServiceModel
    {
        public AllIngredientsFilteredAndPagedServiceModel()
        {
            this.Ingredients = new HashSet<IngredientAllViewModel>();
        }

        public int TotalIngredientsCount { get; set; }

        public ICollection<IngredientAllViewModel> Ingredients { get; set; }
    }
}
