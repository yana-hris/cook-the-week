namespace CookTheWeek.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Data.Models.Recipe;
    using Web.ViewModels.Recipe;

    public interface IRecipeService
    {
        IDictionary<int, int> GenerateServingOptions();
        Task<ICollection<RecipeAllViewModel>> AllUnsortedUnfilteredAsync();

        Task<AllRecipesFilteredAndPagedServiceModel> AllAsync(AllRecipesQueryModel queryModel);
    }
}
