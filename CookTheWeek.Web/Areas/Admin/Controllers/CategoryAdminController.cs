namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Data.SqlClient;
    
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Data.Models;
    using ViewModels.Admin.CategoryAdmin;
    using ViewModels.Category;

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
        private readonly ILogger<CategoryAdminController> logger;

        public CategoryAdminController(ICategoryService<IngredientCategory,
                                            IngredientCategoryAddFormModel,
                                            IngredientCategoryEditFormModel,
                                            IngredientCategorySelectViewModel> ingredientCategoryService,
                                    ICategoryService<RecipeCategory,
                                            RecipeCategoryAddFormModel,
                                            RecipeCategoryEditFormModel,
                                            RecipeCategorySelectViewModel> recipeCategoryService,
                                    ILogger<CategoryAdminController> logger)
        {
            this.recipeCategoryService = recipeCategoryService;
            this.ingredientCategoryService = ingredientCategoryService;
            this.logger = logger;
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
            bool categoryExists = await recipeCategoryService.CategoryExistsByNameAsync(model.Name);
            
            if(categoryExists)
            {
                ModelState.AddModelError(nameof(model.Name), $"Category with name {model.Name} already exists!");
            }

            if(!ModelState.IsValid)
            {
                ICollection<string> modelErrors = ModelState.Values.SelectMany(v => v.Errors)
                                   .Select(e => e.ErrorMessage)
                                   .ToList();
                var formattedErrors = string.Join(Environment.NewLine, modelErrors);
                TempData[ErrorMessage] = formattedErrors;
                return View(model);
            }

            try
            {
                await this.recipeCategoryService.AddCategoryAsync(model);
                TempData[SuccessMessage] = $"Recipe Category with name \"{model.Name}\" was successfully added!";
            }
            catch (Exception)
            {
                logger.LogError($"Recipe Category with name {model.Name} not added!");
                return BadRequest();
            }

            return RedirectToAction("AllRecipeCategories");
        }

        [HttpGet]
        public async Task<IActionResult> EditRecipeCategory(int id)
        {
            bool exists = await recipeCategoryService.CategoryExistsByIdAsync(id);

            if(!exists)
            {
                return NotFound();
            }

            RecipeCategoryEditFormModel model = await this.recipeCategoryService.GetCategoryForEditByIdAsync(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRecipeCategory(RecipeCategoryEditFormModel model)
        {
            bool existsById = await this.recipeCategoryService.CategoryExistsByIdAsync(model.Id);
            if(!existsById)
            {
                return NotFound(model);
            }

            bool existsByName = await this.recipeCategoryService.CategoryExistsByNameAsync(model.Name);
            if(existsByName)
            {
                int? existingCategoryId = await this.recipeCategoryService.GetCategoryIdByNameAsync(model.Name);

                if(existingCategoryId != model.Id)
                {
                    ModelState.AddModelError(nameof(model.Name), $"Recipe Category with name \"{model.Name}\" already exists!");
                }
            }           

            if (ModelState.IsValid)
            {
                try
                {
                    await recipeCategoryService.EditCategoryAsync(model);
                    TempData[SuccessMessage] = $"Recipe Category \"{model.Name}\" edited successfully!";
                    return RedirectToAction("AllRecipeCategories");
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }

            return View(model);
        }
        public async Task<IActionResult> DeleteRecipeCategory(int id)
        {
            bool exists = await this.recipeCategoryService.CategoryExistsByIdAsync(id);

            if(!exists)
            {
                logger.LogError($"Recipe Category with id {id} does not exist.");
                return NotFound();
            }
            
            try
            {
                await this.recipeCategoryService.DeleteCategoryByIdAsync(id);
                TempData[SuccessMessage] = $"Recipe Category successfully deleted!";
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException && (sqlException.Number == 547 || sqlException.Number == 547)) // SQL Server error code for foreign key constraint violation
                {
                    // Handle foreign key constraint violation
                    // Display a message to the user indicating that the deletion cannot be performed due to existing associated records
                    TempData[ErrorMessage] = "Deletion cannot be performed. There are existing associated Recipes with this Category.";
                }
                else
                {
                    // Handle other exceptions
                    TempData[ErrorMessage] = "An error occurred while deleting the category.";
                    logger.LogError($"Recipe Category with id {id} was not deleted.");
                    return BadRequest();
                }
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
            catch (Exception)
            {
                logger.LogError("Ingredient Categories were not loaded.");
                return BadRequest();
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
            bool categoryExists = await this.ingredientCategoryService.CategoryExistsByNameAsync(model.Name);

            if(categoryExists)
            {
                ModelState.AddModelError(nameof(model.Name), $"Category with name {model.Name} already exists!");
            }

            if (!ModelState.IsValid)
            {
                ICollection<string> modelErrors = ModelState.Values.SelectMany(v => v.Errors)
                                   .Select(e => e.ErrorMessage)
                                   .ToList();
                var formattedErrors = string.Join(Environment.NewLine, modelErrors);
                TempData[ErrorMessage] = formattedErrors;
                return View(model);
            }

            try
            {
                await this.ingredientCategoryService.AddCategoryAsync(model);
                TempData[SuccessMessage] = $"Ingredient Category with name \"{model.Name}\" was successfully added!";
            }
            catch (Exception)
            {
                logger.LogError($"Ingredient Category with name {model.Name} was not added.");
                return BadRequest();
            }

            return RedirectToAction("AllIngredientCategories");
        }

        [HttpGet]
        public async Task<IActionResult> EditIngredientCategory(int id)
        {
            bool exists = await ingredientCategoryService.CategoryExistsByIdAsync(id);

            if (!exists)
            {
                logger.LogError($"Ingredient Category with id {id} does not exist.");
                return NotFound();
            }

            try
            {
                IngredientCategoryEditFormModel model = await this.ingredientCategoryService.GetCategoryForEditByIdAsync(id);
                return View(model);
            }
            catch (Exception)
            {
                logger.LogError($"Ingredient Category model for category with {id} was not loaded.");
                return BadRequest();    
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditIngredientCategory(IngredientCategoryEditFormModel model)
        {
            bool existsById = await this.ingredientCategoryService.CategoryExistsByIdAsync(model.Id);
            if (!existsById)
            {
                return NotFound(model);
            }

            bool existsByName = await this.ingredientCategoryService.CategoryExistsByNameAsync(model.Name);
            if (existsByName)
            {
                int? existingCategoryId = await this.ingredientCategoryService.GetCategoryIdByNameAsync(model.Name);

                if (existingCategoryId != model.Id)
                {
                    ModelState.AddModelError(nameof(model.Name), $"Ingredient Category with name \"{model.Name}\" already exists!");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ingredientCategoryService.EditCategoryAsync(model);
                    TempData[SuccessMessage] = $"Ingredient Category \"{model.Name}\" edited successfully!";
                    return RedirectToAction("AllIngredientCategories");
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteIngredientCategory(int id)
        {
            bool exists = await this.ingredientCategoryService.CategoryExistsByIdAsync(id);

            if (!exists)
            {
                logger.LogError($"Ingredient Category with id {id} does not exist and cannot be deleted.");
                return NotFound();
            }

            try
            {
                await this.ingredientCategoryService.DeleteCategoryByIdAsync(id);
                TempData[SuccessMessage] = $"Ingredient Category successfully deleted!";
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException && (sqlException.Number == 547 || sqlException.Number == 547)) // SQL Server error code for foreign key constraint violation
                {
                    // Handle foreign key constraint violation
                    // Display a message to the user indicating that the deletion cannot be performed due to existing associated records
                    TempData[ErrorMessage] = "Deletion cannot be performed. There are existing associated Ingredients with this Category.";
                }
                else
                {
                    // Handle other exceptions
                    TempData[ErrorMessage] = "An error occurred while deleting the category.";
                    logger.LogError($"Ingredient Category with id {id} was not deleted.");
                    return BadRequest();
                }
            }

            return RedirectToAction("AllIngredientCategories");
        }
    }
}
