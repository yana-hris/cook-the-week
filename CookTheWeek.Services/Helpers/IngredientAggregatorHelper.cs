﻿namespace CookTheWeek.Services.Data.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Models.SupplyItem;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.SupplyItem;

    using static CookTheWeek.Common.HelperMethods.IngredientHelper;

    public class IngredientAggregatorHelper : IIngredientAggregatorHelper
    {
        private readonly IRecipeIngredientService recipeIngredientService;

        public IngredientAggregatorHelper(IRecipeIngredientService recipeIngredientService)
        {
            this.recipeIngredientService = recipeIngredientService;
        }


        /// <inheritdoc/>
        public async Task<IEnumerable<ISupplyItemListModel<T>>> AggregateIngredientsByCategory<T>(
                                                                        List<SupplyItemServiceModel> ingredients,
                                                                        Dictionary<string, int[]> categoryDictionary)
            where T : ISupplyItemModel, new()
        {
            var ingredientsByCategories = new List<ISupplyItemListModel<T>>();

            // Load from the DB existing measures and specs
            var measures = await recipeIngredientService.GetRecipeIngredientMeasuresAsync();
            var specifications = await recipeIngredientService.GetRecipeIngredientSpecificationsAsync();

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


       
        /// <inheritdoc/>
        public List<SupplyItemServiceModel> CreateAdjustedIngredientCollection(IEnumerable<RecipeIngredient> recipeIngredients, decimal servingSizeMultiplier)
        {
            var ingredients = recipeIngredients.Select(ri => new SupplyItemServiceModel()
            {
                CategoryId = ri.Ingredient.CategoryId,
                Name = ri.Ingredient.Name,
                Qty = ri.Qty * servingSizeMultiplier,
                MeasureId = ri.MeasureId,
                SpecificationId = ri.SpecificationId
            }).ToList();

            return ingredients;
        }


        /// <inheritdoc/>
        public decimal CalculateServingSizeMultiplier(int desiredServingSize, int defaultServingSize)
        {
            return desiredServingSize * 1.0m / defaultServingSize * 1.0m;
        }

    }
}
