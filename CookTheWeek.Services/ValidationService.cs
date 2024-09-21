namespace CookTheWeek.Services.Data
{
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using CookTheWeek.Services.Data.Interfaces;
    using CookTheWeek.Services.Data.Vlidation;
    using CookTheWeek.Web.ViewModels.Recipe;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;

    using static CookTheWeek.Common.EntityValidationConstants.Recipe;
    using static CookTheWeek.Common.EntityValidationConstants.RecipeIngredient;

    public class ValidationService : IValidationService
    {
        private readonly ICategoryService categoryService;
        private readonly IIngredientService ingredientService;
        private readonly IRecipeIngredientService recipeIngredientService;
        public ValidationService(ICategoryService categoryService,
            IIngredientService ingredientService,
            IRecipeIngredientService recipeIngredientService)
        {
            this.categoryService = categoryService;
            this.ingredientService = ingredientService;
            this.recipeIngredientService = recipeIngredientService;
        }

        // Implement logic to create ingredients which don`t exist!
        public async Task<bool> ValidateIngredientAsync(RecipeIngredientFormModel model)
        {
            bool exists = await this.ingredientService.ExistsByNameAsync(model.Name);
            return exists;
        }

        public async Task<ValidationResult> ValidateRecipeAsync(IRecipeFormModel model)
        {
            var result = new ValidationResult();

            bool categoryExists = await this.categoryService.RecipeCategoryExistsByIdAsync(model.RecipeCategoryId.Value);

            if (!categoryExists)
            {
                AddValidationError(result, nameof(model.RecipeCategoryId), RecipeCategoryIdInvalidErrorMessage);
                
            }

            foreach (var ingredient in model.RecipeIngredients)
            {
                bool exists = await ValidateIngredientAsync(ingredient);

                if (!exists)
                {
                    AddValidationError(result, nameof(ingredient.Name), RecipeIngredientInvalidErrorMessage);
                }

                bool measureExists = await this.recipeIngredientService.IngredientMeasureExistsAsync(ingredient.MeasureId.Value);

                if (!measureExists)
                {
                    AddValidationError(result, nameof(ingredient.MeasureId), MeasureRangeErrorMessage);
                }

                if (ingredient.SpecificationId != null && ingredient.SpecificationId.HasValue)
                {
                    bool specificationExists = await this.recipeIngredientService.IngredientSpecificationExistsAsync(ingredient.SpecificationId.Value);
                    if (!specificationExists)
                    {
                        result.IsValid = false;
                        result.Errors.Add(nameof(ingredient.SpecificationId), SpecificationRangeErrorMessage);
                    }

                }
            }

            return result;
        }
        private static void AddValidationError(ValidationResult result, string key, string errorMessage)
        {
            result.IsValid = false;
            if (!result.Errors.ContainsKey(key))
            {
                result.Errors.Add(key, errorMessage);
            }
        }


    }
}
