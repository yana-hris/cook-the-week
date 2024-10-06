namespace CookTheWeek.Services.Data.Helpers
{

    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.ShoppingList;

    public interface IIngredientAggregatorHelper
    {
        ICollection<ProductListViewModel> AggregateIngredientsByCategory(
        List<ProductServiceModel> ingredients,
        ICollection<RecipeIngredientSelectMeasureViewModel> measures,
        ICollection<RecipeIngredientSelectSpecificationViewModel> specifications);
    }
}
