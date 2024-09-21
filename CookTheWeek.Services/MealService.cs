namespace CookTheWeek.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data.Models;
    using Data.Interfaces;
    using Web.ViewModels.Meal;
    using Web.ViewModels.ShoppingList;
    using Web.ViewModels.Step;

    using static Common.GeneralApplicationConstants;
    using static Common.HelperMethods.IngredientHelper;
    using static Common.ExceptionMessagesConstants.RecordNotFoundExceptionMessages;

    // TODO: Move dbcontext to IMealRepository
    public class MealService : IMealService
    {
        private readonly IMealRepository mealRepository;
        private readonly IRecipeRepository recipeRepository;
        private readonly IRecipeIngredientService recipeIngredientService;

        public MealService(IMealRepository mealRepository, 
            IRecipeRepository recipeRepository,
            IRecipeIngredientService recipeIngredientService)
        {
            this.mealRepository = mealRepository;
            this.recipeRepository = recipeRepository;
            this.recipeIngredientService = recipeIngredientService; 
        }
        public async Task<MealDetailsViewModel> Details(int mealId)
        {
            // Get Meal from mealRepo by Id
            Meal meal = await this.mealRepository.GetByIdAsync(mealId);

            if (meal == null)
            {
                throw new RecordNotFoundException(MealNotFoundExceptionMessage, null);
            }

            string recipeId = meal.RecipeId.ToString();

            // Get RecipeById from recipe (this will throw an exception if not found
            Recipe recipe = await this.recipeRepository.GetByIdAsync(recipeId);

            MealDetailsViewModel model = new MealDetailsViewModel()
            {
                Id = mealId,
                Title = recipe.Title,
                ImageUrl = recipe.ImageUrl,
                Description = recipe.Description,
                CookingTime = recipe.TotalTime,
                CategoryName = recipe.Category.Name,
                CookingSteps = recipe.Steps.Select(st => new StepViewModel()
                {
                    Id = st.Id,
                    Description = st.Description
                }).ToList(),    
            };
            model.ServingSize = meal.ServingSize;

            decimal servingSizeMultiplier = meal.ServingSize * 1.0m / recipe.Servings * 1.0m;

            var ingredients = new List<ProductServiceModel>();

            foreach (var ri in recipe.RecipesIngredients)
            {
                ProductServiceModel ingredient = new ProductServiceModel()
                {
                    Name = ri.Ingredient.Name,
                    MeasureId = ri.MeasureId,
                    Qty = ri.Qty * servingSizeMultiplier,
                    CategoryId = ri.Ingredient.CategoryId,
                    SpecificationId = ri.SpecificationId
                };

                ingredients.Add(ingredient);
            }

            ICollection<ProductListViewModel> ingredientsByCategories = new List<ProductListViewModel>();

            var measures = await recipeIngredientService.GetRecipeIngredientMeasuresAsync();
            var specifications = await recipeIngredientService.GetRecipeIngredientSpecificationsAsync();


            for (int i = 0; i < ProductListCategoryNames.Length; i++)
            {
                int[] categoriesArr = ProductListCategoryIds[i];

                ProductListViewModel ingredientModel = new ProductListViewModel()
                {
                    Title = ProductListCategoryNames[i],
                    Products = ingredients
                            .Where(p => categoriesArr.Contains(p.CategoryId))
                            .Select(p => new ProductViewModel()
                            {
                                Qty = FormatIngredientQty(p.Qty),
                                Measure = measures.Where(m => m.Id == p.MeasureId).Select(m => m.Name).First(),
                                Name = p.Name,
                                Specification = specifications.Where(s => s.Id == p.SpecificationId).Select(s => s.Name).FirstOrDefault()
                            }).ToList()
                };

                ingredientsByCategories.Add(ingredientModel);
            }
            model.IngredientsByCategories = ingredientsByCategories;
            return model;
        }
       
    }
}
