namespace CookTheWeek.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    
    using Web.ViewModels.Meal;
    using Web.ViewModels.Recipe;

    public interface IRecipeService
    {
        Task<ICollection<RecipeAllViewModel>> AllAsync(AllRecipesQueryModel queryModel, string userId);
        Task<string> AddAsync(RecipeAddFormModel model, string ownerId, bool isAdmin);
        Task<RecipeDetailsViewModel> DetailsByIdAsync(string id, string userId);
        Task<bool> ExistsByIdAsync(string id);
        Task<RecipeEditFormModel> GetForEditByIdAsync(string id);
        Task EditAsync(RecipeEditFormModel model);
        Task DeleteByIdAsync(string id);
        Task<ICollection<RecipeAllViewModel>> AllAddedByUserAsync(string userId);
        Task<int> MineCountAsync(string userId);         
        Task<int> AllCountAsync();
        Task<bool> IsIncludedInMealPlans(string id);
        Task<MealAddFormModel> GetForMealByIdAsync(string recipeId);
        Task<ICollection<RecipeAllViewModel>> AllSiteAsync();
        Task<ICollection<RecipeAllViewModel>> AllUserRecipesAsync();
    }
}
