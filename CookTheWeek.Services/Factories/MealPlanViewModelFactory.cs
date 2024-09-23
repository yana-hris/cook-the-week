namespace CookTheWeek.Services.Data.Factories
{
    using System.Threading.Tasks;

    using CookTheWeek.Services.Data.Factories.Interfaces;
    using CookTheWeek.Services.Data.Interfaces;
    using CookTheWeek.Web.ViewModels.Recipe;

    public class MealPlanViewModelFactory : IMealplanViewModelFactory
    {
        private readonly IRecipeService recipeService;

        public MealPlanViewModelFactory()
        {
            
        }
        public Task<RecipeAddFormModel> CreateRecipeAddFormModelAsync()
        {
            throw new NotImplementedException();
        }
    }
}
