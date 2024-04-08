namespace CookTheWeek.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Data.Models.Recipe;
    using Web.ViewModels.Recipe;

    public interface IRecipeService
    {
        Task<AllRecipesFilteredAndPagedServiceModel> AllAsync(AllRecipesQueryModel queryModel);
        Task AddAsync(RecipeAddFormModel model, string ownerId);
        Task<RecipeDetailsViewModel> DetailsByIdAsync(string id);
        Task<bool> ExistsByIdAsync(string id);
        Task<RecipeEditFormModel> GetForEditByIdAsync(string id);
        Task EditAsync(RecipeEditFormModel model);
        Task<RecipeDeleteViewModel> GetForDeleteByIdAsync(string id);
        Task DeleteById(string id);
        Task<ICollection<RecipeAllViewModel>> AllAdedByUserAsync(string userId);
        Task<int> MineCountAsync(string userId);    
        Task<bool> IsOwner(string id, string ownerId);
        Task<bool> IsFavouriteRecipeForUserByIdAsync(string id, string userId);
        Task AddToFavouritesByUserId(string id, string userId);
        Task RemoveFromFavouritesByUserId(string id, string userId);
        Task<int> AllCountAsync();
        Task<ICollection<RecipeAllViewModel>> AllFavouritesByUserAsync(string userId);
        Task<bool> IsIncludedInMealPlans(string id);
    }
}
