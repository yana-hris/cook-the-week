namespace CookTheWeek.Services.Interfaces
{
    using CookTheWeek.Web.ViewModels.Recipe;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRecipeService
    {
        Task<ICollection<RecipeAllViewModel>> GetAllRecipesAsync();
    }
}
