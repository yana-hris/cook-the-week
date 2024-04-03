namespace CookTheWeek.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Data.Models.Recipe;
    using Web.ViewModels.Recipe;

    public interface IRecipeService
    {
        Task<ICollection<RecipeAllViewModel>> AllUnsortedUnfilteredAsync();
        Task<AllRecipesFilteredAndPagedServiceModel> AllAsync(AllRecipesQueryModel queryModel);
        Task AddAsync(RecipeFormViewModel model, string ownerId);
        Task<RecipeDetailsViewModel> DetailsByIdAsync(string id);
        Task<bool> ExistsByIdAsync(string id);
        Task<RecipeEditViewModel> GetForEditByIdAsync(string id);
        Task EditAsync(RecipeEditViewModel model);
        Task<RecipeDeleteViewModel> GetForDeleteByIdAsync(string id);
        Task DeleteById(string id);
        Task<ICollection<RecipeAllViewModel>> Mine(string userId);
        Task<bool> IsOwner(string id, string ownerId);
        Task<bool> IsFavouriteRecipeForUserByIdAsync(string id, string userId);

        Task AddToFavouritesByUserId(string id, string userId);
        Task RemoveFromFavouritesByUserId(string id, string userId);
    }
}
