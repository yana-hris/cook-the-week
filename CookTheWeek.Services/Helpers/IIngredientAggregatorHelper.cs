namespace CookTheWeek.Services.Data.Helpers
{
    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Models.SupplyItem;
    using CookTheWeek.Web.ViewModels.Interfaces;

    public interface IIngredientAggregatorHelper
    {

        /// <summary>
        /// Aggregates a list of ingredients by their respective categories, and returns a collection of supply item list models grouped by category.
        /// </summary>
        /// <typeparam name="T">The type of the supply item model, which must implement ISupplyItemModel and have a parameterless constructor.</typeparam>
        /// <param name="ingredients">The list of ingredients to be aggregated.</param>
        /// <param name="categoryDictionary">A dictionary where the key is the category name and the value is an array of category IDs.</param>
        /// <returns>A collection of ISupplyItemListModel grouped by category, containing the aggregated ingredients with measures and specifications.</returns>
        Task<IEnumerable<ISupplyItemListModel<T>>> AggregateIngredientsByCategory<T>(List<SupplyItemServiceModel> ingredients, Dictionary<string, int[]> categoryDictionary)
            where T : ISupplyItemModel, new();

        /// <summary>
        /// Processes a collection of recipe ingredients, applying a serving size multiplier to adjust quantities, and returns a list of SupplyItemServiceModel objects.
        /// </summary>
        /// <param name="recipeIngredients">The collection of recipe ingredients to process.</param>
        /// <param name="servingSizeMultiplier">The multiplier applied to the ingredient quantities to adjust for different serving sizes.</param>
        /// <returns>A list of SupplyItemServiceModel objects, each representing an ingredient with its adjusted quantity, measure, and specification.</returns>
        List<SupplyItemServiceModel> CreateAdjustedIngredientCollection(IEnumerable<RecipeIngredient> recipeIngredients, decimal servingSizeMultiplier);


        /// <summary>
        /// Calculates the serving size multiplier, used for calculating the actual Qty needed to cook a meal
        /// </summary>
        /// <param name="desiredServingSize"></param>
        /// <param name="defaultServingSize"></param>
        /// <returns></returns>
        decimal CalculateServingSizeMultiplier(int desiredServingSize, int defaultServingSize);
    }
}
