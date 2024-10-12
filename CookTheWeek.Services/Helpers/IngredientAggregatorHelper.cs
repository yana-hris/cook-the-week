namespace CookTheWeek.Services.Data.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    using CookTheWeek.Services.Data.Models.SupplyItem;
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.SupplyItem;

    using static CookTheWeek.Common.HelperMethods.IngredientHelper;

    public class IngredientAggregatorHelper : IIngredientAggregatorHelper
    {
        public IEnumerable<ISupplyItemListModel<T>> AggregateIngredientsByCategory<T>(
            List<SupplyItemServiceModel> ingredients,
            IEnumerable<ISelectViewModel> measures,
            IEnumerable<ISelectViewModel> specifications,
            Dictionary<string, int[]> categoryDictionary
            )
            where T : ISupplyItemModel, new()
        {
            var ingredientsByCategories = new List<ISupplyItemListModel<T>>();

            // Preprocess measures and specifications into dictionaries for fast lookup
            var measureDict = measures.ToDictionary(m => m.Id, m => m.Name);
            var specificationDict = specifications.ToDictionary(sp => sp.Id, sp => sp.Name);

            // Group ingredients by category for faster lookup
            var ingredientsGroupedByCategory = ingredients
                .GroupBy(p => p.CategoryId)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Iterate over each category array to build the product list
            foreach (var categoryEntry in categoryDictionary)
            {
                string categoryName = categoryEntry.Key;  // Category name
                int[] categoryIds = categoryEntry.Value;  // Array of category IDs

                var productsInCategory = new List<T>();

                // Collect ingredients that belong to the current category
                foreach (var categoryId in categoryIds)
                {
                    if (ingredientsGroupedByCategory.TryGetValue(categoryId, out var ingredientsInCategory))
                    {
                        productsInCategory.AddRange(ingredientsInCategory.Select(p => new T
                        {
                            Qty = FormatIngredientQty(p.Qty),
                            Measure = measureDict.TryGetValue(p.MeasureId, out var measureName) ? measureName : "N/A",
                            Name = p.Name,
                            Specification = p.SpecificationId.HasValue ?
                                           specificationDict.TryGetValue(p.SpecificationId.Value, out var specName) ?
                                           specName : ""
                                           : ""
                        }));
                    }
                }

                // Add the product list to the category view model
                var productListViewModel = new SupplyItemListModel<T>
                {
                    Title = categoryName,   // Assuming index is the same
                    SupplyItems = productsInCategory
                };

                ingredientsByCategories.Add(productListViewModel);
            }

            return ingredientsByCategories;
        }
    }
}
