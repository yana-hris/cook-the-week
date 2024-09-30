namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Services.Data.Models.Ingredient;
    using CookTheWeek.Web.ViewModels.Admin.IngredientAdmin;
    using CookTheWeek.Web.ViewModels.Admin.IngredientAdmin.Enums;
    using CookTheWeek.Web.ViewModels.Admin.CategoryAdmin;
    using CookTheWeek.Web.ViewModels.Category;

    using static Common.NotificationMessagesConstants;

    public class IngredientAdminController : BaseAdminController
    {
        private readonly ICategoryService<IngredientCategory,
                                            IngredientCategoryAddFormModel,
                                            IngredientCategoryEditFormModel,
                                            IngredientCategorySelectViewModel> categoryService;
        private readonly IIngredientService ingredientService;

        public IngredientAdminController(ICategoryService<IngredientCategory,
                                            IngredientCategoryAddFormModel,
                                            IngredientCategoryEditFormModel,
                                            IngredientCategorySelectViewModel> categoryService,
            IIngredientService ingredientService,
            ILogger<IngredientAdminController> logger) : base(logger)
        {
            this.categoryService = categoryService;
            this.ingredientService = ingredientService;

        }

        [HttpGet]
        public async Task<IActionResult> All([FromQuery] AllIngredientsQueryModel queryModel)
        {
            try
            {
                AllIngredientsFilteredAndPagedServiceModel serviceModel = await ingredientService
                .AllAsync(queryModel);
                queryModel.Ingredients = serviceModel.Ingredients;
                queryModel.TotalIngredients = serviceModel.TotalIngredientsCount;
                queryModel.Categories = await categoryService.GetAllCategoryNamesAsync();
                queryModel.IngredientSortings = Enum.GetValues(typeof(IngredientSorting))
                    .Cast<IngredientSorting>()
                    .ToDictionary(rs => (int)rs, rs => rs.ToString());

                return View(queryModel);
            }
            catch (Exception)
            {
                logger.LogError($"Ingredient Query model was not successfully loaded.");
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            IngredientAddFormModel model = new IngredientAddFormModel();
            model.IngredientCategories = await categoryService.GetAllCategoriesAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm]IngredientAddFormModel model)
        {
            bool ingredientExists = await ingredientService.ExistsByNameAsync(model.Name);
            bool categoryExists = await categoryService.CategoryExistsByIdAsync(model.CategoryId);

            if (ingredientExists)
            {
                ModelState.AddModelError(nameof(model.Name), $"Ingredient with name \"{model.Name}\" already exists!");
            }

            if (!categoryExists)
            {
                ModelState.AddModelError(nameof(model.CategoryId), $"Invalid Ingredient Category: {model.CategoryId}");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ingredientService.AddAsync(model);
                    TempData[SuccessMessage] = $"Ingredient \"{model.Name}\" added successfully!";
                    return RedirectToAction("All");
                }
                catch (Exception)
                {
                    logger.LogError($"Ingredient with name {model.Name} was not successfully added.");
                    return BadRequest();
                }
            }

            model.IngredientCategories = await categoryService.GetAllCategoriesAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            bool exists = await this.ingredientService.ExistsByIdAsync(id);

            if(!exists)
            {
                logger.LogError($"Ingredient with id {id} does not exist and cannot be edited.");
                return NotFound();
            }

            try
            {
                IngredientEditFormModel model = await ingredientService.GetForEditByIdAsync(id);
                model.Categories = await categoryService.GetAllCategoriesAsync();
                return View(model);
            }
            catch (Exception)
            {
                logger.LogError($"Model for ingredient with id {id} was not successfully loaded.");
                return BadRequest();
            }
        }

        // TODO: Refactor controller and use exceptions thrown from service anyway
        [HttpPost]
        public async Task<IActionResult> Edit(IngredientEditFormModel model)
        {
            bool ingredientExists = await this.ingredientService.ExistsByIdAsync(model.Id);

            if(!ingredientExists)
            {
                logger.LogError($"Ingredient with id {model.Id} does not exist and cannot be edited.");
                return NotFound();
            }

            bool nameAlreadyExists = await ingredientService.ExistsByNameAsync(model.Name);

            if (nameAlreadyExists)
            {
                int existingIngredientId = await ingredientService.GetIdByNameAsync(model.Name);

                if(existingIngredientId != model.Id)
                {
                    ModelState.AddModelError(nameof(model.Name), $"Ingredient with name \"{model.Name}\" already exists!");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ingredientService.EditAsync(model);
                    TempData[SuccessMessage] = $"Ingredient \"{model.Name}\" edited successfully!";
                    return RedirectToAction("All");
                }
                catch (Exception)
                {
                    logger.LogError($"Ingredient with id {model.Id} was not edited successfully");
                    return BadRequest();
                }
            }

            model.Categories = await categoryService.GetAllCategoriesAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await this.ingredientService.TryDeleteByIdAsync(id);
                TempData[SuccessMessage] = "Ingredient successfully deleted!";
            }
            catch (RecordNotFoundException)
            {
                return RedirectToAction("NotFound", "Home", new { area = "" });
            }
            catch(InvalidOperationException ex)
            {
                TempData[ErrorMessage] = ex.Message;
            }
            catch (Exception ex)
            {
                logger.log
            }

            return RedirectToAction("All");
        }
    }
}
