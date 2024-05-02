namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Data.SqlClient;

    using Services.Data.Interfaces;
    using Services.Data.Models.Ingredient;
    using ViewModels.Admin.IngredientAdmin;
    using ViewModels.Admin.IngredientAdmin.Enums;

    using static Common.NotificationMessagesConstants;

    public class IngredientAdminController : BaseAdminController
    {
        private readonly ICategoryService categoryService;
        private readonly IIngredientService ingredientService;
        private readonly ILogger<IngredientAdminController> logger;

        public IngredientAdminController(ICategoryService categoryService,
            IIngredientService ingredientService,
            ILogger<IngredientAdminController> logger)
        {
            this.categoryService = categoryService;
            this.ingredientService = ingredientService;
            this.logger = logger;
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
                queryModel.Categories = await categoryService.AllIngredientCategoryNamesAsync();
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
            model.IngredientCategories = await categoryService.AllIngredientCategoriesAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm]IngredientAddFormModel model)
        {
            bool ingredientExists = await ingredientService.ExistsByNameAsync(model.Name);
            bool categoryExists = await categoryService.IngredientCategoryExistsByIdAsync(model.CategoryId);

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

            model.IngredientCategories = await categoryService.AllIngredientCategoriesAsync();
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
                model.Categories = await categoryService.AllIngredientCategoriesAsync();
                return View(model);
            }
            catch (Exception)
            {
                logger.LogError($"Model for ingredient with id {id} was not successfully loaded.");
                return BadRequest();
            }
        }

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

            model.Categories = await categoryService.AllIngredientCategoriesAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            bool exists = await this.ingredientService.ExistsByIdAsync(id);

            if(!exists)
            {
                logger.LogError($"Ingredient with id {id} does not exist and cannot be deleted.");
                return NotFound();
            }
            
            try
            {
                await this.ingredientService.DeleteById(id);
                TempData[SuccessMessage] = "Ingredient successfully deleted!";
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException && (sqlException.Number == 547 || sqlException.Number == 547)) // SQL Server error code for foreign key constraint violation
                {
                    // Handle foreign key constraint violation
                    // Display a message to the user indicating that the deletion cannot be performed due to existing associated records
                    TempData[ErrorMessage] = "Deletion cannot be performed. There are existing associated RecipeIngredients with this Ingredient.";
                }
                else
                {
                    // Handle other exceptions
                    TempData[ErrorMessage] = "An error occurred while deleting the Ingredient.";
                    logger.LogError($"Ingredient with id {id} was not edited successfully.");
                    return BadRequest();
                }
            }

            return RedirectToAction("All");
        }
    }
}
