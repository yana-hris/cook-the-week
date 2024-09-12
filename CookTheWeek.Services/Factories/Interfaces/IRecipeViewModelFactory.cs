namespace CookTheWeek.Services.Data.Factories.Interfaces
{
    using CookTheWeek.Web.ViewModels.Recipe;

    public interface IRecipeViewModelFactory
    {
        Task<AllRecipesFilteredAndPagedViewModel> CreateAllRecipesViewModelAsync(AllRecipesQueryModel queryModel, string userId);
    }
}
