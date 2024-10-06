namespace CookTheWeek.Services.Data.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.ShoppingList;

    using static CookTheWeek.Common.HelperMethods.IngredientHelper;
    using static CookTheWeek.Common.GeneralApplicationConstants;


    public class IngredientAggregatorHelper : IIngredientAggregatorHelper
    {
        public ICollection<ProductListViewModel> AggregateIngredientsByCategory(
            List<ProductServiceModel> ingredients,
            ICollection<RecipeIngredientSelectMeasureViewModel> measures,
            ICollection<RecipeIngredientSelectSpecificationViewModel> specifications)
        {
            var ingredientsByCategories = new List<ProductListViewModel>();

            // Preprocess measures and specifications into dictionaries for fast lookup
            var measureDict = measures.ToDictionary(m => m.Id, m => m.Name);
            var specificationDict = specifications.ToDictionary(sp => sp.Id, sp => sp.Name);

            // Group ingredients by category for faster lookup
            var ingredientsGroupedByCategory = ingredients
                .GroupBy(p => p.CategoryId)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Iterate over each category array to build the product list
            foreach (var categoryIds in ProductListCategoryIds)
            {
                var productsInCategory = new List<ProductViewModel>();

                // Collect ingredients that belong to the current category
                foreach (var categoryId in categoryIds)
                {
                    if (ingredientsGroupedByCategory.TryGetValue(categoryId, out var ingredientsInCategory))
                    {
                        productsInCategory.AddRange(ingredientsInCategory.Select(p => new ProductViewModel
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
                var productListViewModel = new ProductListViewModel
                {
                    Title = ProductListCategoryNames[ProductListCategoryIds.ToList().IndexOf(categoryIds)], // Assuming index is the same
                    Products = productsInCategory
                };

                ingredientsByCategories.Add(productListViewModel);
            }

            return ingredientsByCategories;
        }
    }
}
