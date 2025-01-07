namespace CookTheWeek.Services.Data.Helpers
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

            // Preprocess measures and specifications into dictionaries for fast lookup
            var measureDict = measures.ToDictionary(m => m.Id, m => m.Name);

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
                        // Group ingredients by name first
                        var groupedByName = ingredientsInCategory
                            .GroupBy(i => i.Name.ToLower())
                            .Select(g => new
                            {
                                Name = g.Key,
                                Measures = g.GroupBy(m => m.MeasureId)
                                    .Select(mg => new
                                    {
                                        Measure = measureDict.TryGetValue(mg.Key, out var measure) ? measure : "N/A",
                                        TotalQty = mg.Sum(i => i.Qty),
                                        Notes = mg.Select(i => new T
                                        {
                                            Name = null,
                                            Qty = FormatIngredientQty(i.Qty),
                                            Measure = measureDict.TryGetValue(mg.Key, out var measure) ? measure : "N/A",
                                            Note = string.IsNullOrEmpty(i.Note) ? null : i.Note,
                                            ChildItems = null
                                        }).ToList()
                                    })
                                    .OrderBy(m => m.Measure) // Order measures alphabetically
                                    .ToList()
                            });

                        foreach (var product in groupedByName)
                        {
                            // Create parent item (Product Name)
                            var parentProduct = new T
                            {
                                Name = product.Name,
                                Qty = null,
                                Measure = null,
                                Note = null,
                                ChildItems = product.Measures.Select(m => new T
                                {
                                    Name = null,
                                    Qty = FormatIngredientQty(m.TotalQty),
                                    Measure = m.Measure,
                                    Note = null,
                                    ChildItems = m.Notes
                                        .Cast<ISupplyItemModel>()
                                        .ToList() 
                                })
                                .Cast<ISupplyItemModel>()
                                .ToList()
                            };

                            productsInCategory.Add(parentProduct);
                           
                        }
                    }
                }

                if (productsInCategory.Any())
                {
                    ingredientsByCategories.Add(new SupplyItemListModel<T>
                    {
                        Title = categoryName,
                        SupplyItems = productsInCategory
                    });
                }
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
                Note = ri.Note
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
