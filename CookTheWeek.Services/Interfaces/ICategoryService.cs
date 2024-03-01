namespace CookTheWeek.Services.Interfaces
{
    using CookTheWeek.Web.ViewModels.Category;

    public interface ICategoryService
    {
        Task<ICollection<IngredientCategorySelectViewModel>> AllIngredientCategoriesAsync();

        Task<ICollection<RecipeCategorySelectViewModel>> AllRecipeCategoriesAsync();
        Task<bool> IngredientCategoryExistsByIdAsync(int ingredientCategoryId);

        Task<ICollection<string>> AllRecipeCategoryNamesAsync();


    }
}
