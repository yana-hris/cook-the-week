namespace CookTheWeek.Services.Interfaces
{
    using CookTheWeek.Web.ViewModels.Category;

    public interface ICategoryService
    {
        Task<ICollection<IngredientCategorySelectViewModel>> GetAllIngredientCategoriesAsync();

        Task<ICollection<RecipeCategorySelectViewModel>> GetAllRecipeCategoriesAsync();
    }
}
