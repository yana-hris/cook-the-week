namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Services.Data.Models.Ingredient;
    using CookTheWeek.Web.ViewModels;
    using CookTheWeek.Web.ViewModels.Admin.IngredientAdmin;
    using CookTheWeek.Web.ViewModels.Admin.IngredientAdmin.Enums;
    using CookTheWeek.Web.ViewModels.Admin.CategoryAdmin;
    using CookTheWeek.Web.ViewModels.Interfaces;

    using static Common.NotificationMessagesConstants;
    using static Common.EntityValidationConstants;
    using CookTheWeek.Web.ViewModels.Recipe.Enums;

    public class IngredientAdminController : BaseAdminController
    {
        private readonly ICategoryService<IngredientCategory,
                                            IngredientCategoryAddFormModel,
                                            IngredientCategoryEditFormModel,
                                            SelectViewModel> categoryService;
        private readonly IIngredientService ingredientService;

        public IngredientAdminController(ICategoryService<IngredientCategory,
                                            IngredientCategoryAddFormModel,
                                            IngredientCategoryEditFormModel,
                                            SelectViewModel> categoryService,
            IIngredientService ingredientService,
            ILogger<IngredientAdminController> logger) 
        : base(logger)
        {
            this.categoryService = categoryService;
            this.ingredientService = ingredientService;

        }

        [HttpGet]
        public async Task<IActionResult> All([FromQuery] AllIngredientsQueryModel queryModel)
        {
            try
            {
                var model = await ingredientService
                .AllAsync(queryModel);                
                queryModel.Categories = await categoryService.GetAllCategoriesAsync();

                return View(queryModel);
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(All), "Ingredient", null);                
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            IngredientAddFormModel model = new IngredientAddFormModel();
            await PopulateIngredientCategoriesAsync(model);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm]IngredientAddFormModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateIngredientCategoriesAsync(model);
                return View(model);
            }

            try
            {
                var result = await ingredientService.TryAddIngredientAsync(model);

                if (result.Succeeded)
                {
                    TempData[SuccessMessage] = IngredientValidation.IngredientSuccessfullyAddedMessage;

                    return RedirectToAction("All");
                }

                AddCustomValidationErrorsToModelState(result.Errors);
                await PopulateIngredientCategoriesAsync(model);
                return View(model);
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(Add), nameof(Ingredient), null);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                IngredientEditFormModel model = await ingredientService.TryGetIngredientModelForEditAsync(id);
                await PopulateIngredientCategoriesAsync(model);
                return View(model);
            }
            catch (RecordNotFoundException ex)
            {
                TempData[ErrorMessage] = ex.Message;

                return RedirectToAction("All");
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(Edit), nameof(Ingredient), id.ToString());
            }
        }

        
        [HttpPost]
        public async Task<IActionResult> Edit(IngredientEditFormModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateIngredientCategoriesAsync(model);
                return View(model);
            }

            try
            {
                var result = await ingredientService.TryEditIngredientAsync(model);

                if (!result.Succeeded)
                {
                    AddCustomValidationErrorsToModelState(result.Errors);
                    await PopulateIngredientCategoriesAsync(model);
                    return View(model);
                }

                TempData[SuccessMessage] = IngredientValidation.IngredientSuccessfullyEditedMessage;
            }
            catch (RecordNotFoundException ex)
            {
                TempData[ErrorMessage] = ex.Message;
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(Edit), nameof(Ingredient), model.Id.ToString());
            }

            return RedirectToAction("All");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await ingredientService.TryDeleteByIdAsync(id);
                TempData[SuccessMessage] = IngredientValidation.IngredientSuccessfullyDeletedMessage;
            }
            catch (RecordNotFoundException ex)
            {
                TempData[ErrorMessage] = ex.Message;
            }
            catch(InvalidOperationException ex)
            {
                TempData[ErrorMessage] = ex.Message;
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(Delete), nameof(Ingredient), id.ToString());
            }

            return RedirectToAction("All");
        }

        /// <summary>
        /// Helper method to populate Ingredient Categories for Select menu
        /// </summary>
        /// <param name="model">IIngredientFormModel for adding or editing an Ingredient</param>
        /// <returns></returns>
        private async Task PopulateIngredientCategoriesAsync(IIngredientFormModel model)
        {
            model.Categories = await categoryService.GetAllCategoriesAsync();
        }

        


    }
}
