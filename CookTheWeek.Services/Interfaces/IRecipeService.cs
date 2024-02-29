namespace CookTheWeek.Services.Interfaces
{
    using CookTheWeek.Web.ViewModels.Recipe;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRecipeService
    {
        IDictionary<int, int> GenerateServingOptions();
        Task<ICollection<RecipeAllViewModel>> GetAllRecipesAsync();
    }
}
