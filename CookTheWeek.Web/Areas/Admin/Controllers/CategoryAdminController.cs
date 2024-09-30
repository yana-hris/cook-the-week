namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    
    using CookTheWeek.Common;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Admin.CategoryAdmin;
    using CookTheWeek.Web.ViewModels.Category;

    using static Common.NotificationMessagesConstants;

    public class CategoryAdminController : BaseAdminController
    {
        private readonly ICategoryService<IngredientCategory,
                                            IngredientCategoryAddFormModel,
                                            IngredientCategoryEditFormModel,
                                            IngredientCategorySelectViewModel> ingredientCategoryService;
        private readonly ICategoryService<RecipeCategory,
                                            RecipeCategoryAddFormModel,
                                            RecipeCategoryEditFormModel,
                                            RecipeCategorySelectViewModel> recipeCategoryService;
        private readonly IValidationService validationService;

        public CategoryAdminController(ICategoryService<IngredientCategory,
                                            IngredientCategoryAddFormModel,
                                            IngredientCategoryEditFormModel,
                                            IngredientCategorySelectViewModel> ingredientCategoryService,
                                    ICategoryService<RecipeCategory,
                                            RecipeCategoryAddFormModel,
                                            RecipeCategoryEditFormModel,
                                            RecipeCategorySelectViewModel> recipeCategoryService,
                                    IValidationService validationService,
                                    ILogger<CategoryAdminController> logger) : base(logger)
        {
            this.recipeCategoryService = recipeCategoryService;
            this.ingredientCategoryService = ingredientCategoryService;
            this.validationService = validationService;
        }

        // Recipe Category Service
        [HttpGet]
        public async Task<IActionResult> AllRecipeCategories()
        {
            ICollection<RecipeCategorySelectViewModel> all = await this.recipeCategoryService
                .GetAllCategoriesAsync();

            return View(all);
        }

        [HttpGet]
        public IActionResult AddRecipeCategory()
        {
            RecipeCategoryAddFormModel model = new RecipeCategoryAddFormModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddRecipeCategory(RecipeCategoryAddFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            try
            {
                OperationResult result = await this.recipeCategoryService.TryAddCategoryAsync(model);

                if (!result.Succeeded) 
                {
                    AddCustomValidationErrorsToModelState(result.Errors);
                    return View(model);
                }

                TempData[SuccessMessage] = $"Recipe Category with name \"{model.Name}\" was successfully added!";
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(AddRecipeCategory), nameof(RecipeCategory), null);
            }

            return RedirectToAction("AllRecipeCategories");
        }

        [HttpGet]
        public async Task<IActionResult> EditRecipeCategory(int id)
        {
            try
            {
                RecipeCategoryEditFormModel model = await this.recipeCategoryService.TryGetCategoryModelForEditAsync(id);
                return View(model);

            }
            catch (RecordNotFoundException ex)
            {
                TempData[ErrorMessage] = ex.Message;
            }
            catch(Exception ex)
            {
                return HandleException(ex, nameof(EditRecipeCategory), nameof(RecipeCategory), id.ToString());
            }

            return RedirectToAction("AllRecipeCategories");
            
        }

        [HttpPost]
        public async Task<IActionResult> EditRecipeCategory(RecipeCategoryEditFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = await recipeCategoryService.TryEditCategoryAsync(model);

                if (result.Succeeded)
                {
                    TempData[SuccessMessage] = $"Recipe Category \"{model.Name}\" edited successfully!";
                }
                else
                {
                    AddCustomValidationErrorsToModelState(result.Errors);
                    return View(model);
                }
            }
            catch (RecordNotFoundException ex)
            {
                TempData[ErrorMessage] = ex.Message;
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(EditRecipeCategory), nameof(RecipeCategory), model.Id.ToString());
            }

            return RedirectToAction("AllRecipeCategories");

        }

        [HttpGet]
        public async Task<IActionResult> DeleteRecipeCategory(int id)
        {
            try
            {
                await recipeCategoryService.TryDeleteCategoryAsync(id);
                TempData[SuccessMessage] = $"Recipe Category successfully deleted!";
            }
            catch (RecordNotFoundException)
            {
                TempData[ErrorMessage] = "Recipe Category does not exist.";
            }
            catch (InvalidOperationException)
            {
                TempData[ErrorMessage] = "Recipe Category cannot be deleted due to existing Recipes.";
            }
            catch(Exception ex)
            {
                return HandleException(ex, nameof(DeleteRecipeCategory), nameof(RecipeCategory), id.ToString());
            }

            return RedirectToAction("AllRecipeCategories");
        }

        // Ingredient Category Service
        public async Task<IActionResult> AllIngredientCategories()
        {
            try
            {
                ICollection<IngredientCategorySelectViewModel> all = await ingredientCategoryService
                                                                        .GetAllCategoriesAsync();
                return View(all);
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(AllIngredientCategories), nameof(IngredientCategory), null);
            }
        }

        [HttpGet]
        public IActionResult AddIngredientCategory()
        {
            IngredientCategoryAddFormModel model = new IngredientCategoryAddFormModel();
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddIngredientCategory(IngredientCategoryAddFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                OperationResult result = await ingredientCategoryService.TryAddCategoryAsync(model);

                if (!result.Succeeded)
                {
                    AddCustomValidationErrorsToModelState(result.Errors);
                    return View(model);
                }

                TempData[SuccessMessage] = $"Ingredient Category with name \"{model.Name}\" was successfully added!";
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(AddRecipeCategory), nameof(RecipeCategory), null);
            }

            return RedirectToAction("AllIngredientCategories");
        }

        [HttpGet]
        public async Task<IActionResult> EditIngredientCategory(int id)
        {
            try
            {
                IngredientCategoryEditFormModel model = await this.ingredientCategoryService.TryGetCategoryModelForEditAsync(id);
                return View(model);

            }
            catch (RecordNotFoundException ex)
            {
                TempData[ErrorMessage] = ex.Message;
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(EditIngredientCategory), nameof(IngredientCategory), id.ToString());
            }

            return RedirectToAction("AllIngredientCategories");

        }

        [HttpPost]
        public async Task<IActionResult> EditIngredientCategory(IngredientCategoryEditFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                OperationResult result = await ingredientCategoryService.TryEditCategoryAsync(model);

                if (result.Succeeded)
                {
                    TempData[SuccessMessage] = $"Ingredient Category \"{model.Name}\" edited successfully!";
                }
                else
                {
                    AddCustomValidationErrorsToModelState(result.Errors);
                    return View(model);
                }
            }
            catch (RecordNotFoundException ex)
            {
                TempData[ErrorMessage] = ex.Message;
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(EditIngredientCategory), nameof(IngredientCategory), model.Id.ToString());
            }

            return RedirectToAction("AllIngredientCategories");

        }

        public async Task<IActionResult> DeleteIngredientCategory(int id)
        {
            try
            {
                await ingredientCategoryService.TryDeleteCategoryAsync(id);
                TempData[SuccessMessage] = $"Ingredient Category successfully deleted!";
            }
            catch (RecordNotFoundException ex)
            {
                TempData[ErrorMessage] = ex.Message;
            }
            catch (InvalidOperationException ex)
            {
                TempData[ErrorMessage] = ex.Message;
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(DeleteIngredientCategory), nameof(IngredientCategory), id.ToString());
            }

            return RedirectToAction("AllIngredientCategories");

           
        }
    }
}
