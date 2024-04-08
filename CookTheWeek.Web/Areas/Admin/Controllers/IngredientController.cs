namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Data.SqlClient;

    using Services.Interfaces;
    using Services.Data.Models.Ingredient;
    using Web.ViewModels.Ingredient;
    using Web.ViewModels.Ingredient.Enums;

    using static Common.NotificationMessagesConstants;
    

    public class IngredientController : BaseAdminController
    {
        private readonly ICategoryService categoryService;
        private readonly IIngredientService ingredientService;

        public IngredientController(ICategoryService categoryService,
            IIngredientService ingredientService)
        {
            this.categoryService = categoryService;
            this.ingredientService = ingredientService;
        }

        [HttpGet]
        public async Task<IActionResult> All([FromQuery] AllIngredientsQueryModel queryModel)
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

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            IngredientFormViewModel model = new IngredientFormViewModel();
            model.IngredientCategories = await categoryService.AllIngredientCategoriesAsync();


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([FromForm]IngredientFormViewModel model)
        {
            bool ingredientExists = await ingredientService.ExistsByNameAsync(model.Name);
            bool categoryExists = await categoryService.IngredientCategoryExistsByIdAsync(model.IngredientCategoryId);

            if (ingredientExists)
            {
                ModelState.AddModelError(nameof(model.Name), $"Ingredient with name \"{model.Name}\" already exists!");
            }

            if (!categoryExists)
            {
                ModelState.AddModelError(nameof(model.IngredientCategoryId), $"Invalid Ingredient Category: {model.IngredientCategoryId}");
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
                return NotFound();
            }

            IngredientEditViewModel model = await ingredientService.GetForEditByIdAsync(id);
            model.Categories = await categoryService.AllIngredientCategoriesAsync();

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IngredientEditViewModel model)
        {
            bool ingredientExists = await this.ingredientService.ExistsByIdAsync(model.Id);

            if(!ingredientExists)
            {
                return NotFound();
            }

            bool nameAlreadyExists = await ingredientService.ExistsByNameAsync(model.Name);

            if (nameAlreadyExists)
            {
                int existingIngredientId = await ingredientService.GetIdByName(model.Name);

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
                    return BadRequest();
                }
            }

            model.Categories = await categoryService.AllIngredientCategoriesAsync();
            return View(model);
        }

       
        public async Task<IActionResult> Delete(int id)
        {
            bool exists = await this.ingredientService.ExistsByIdAsync(id);

            if(!exists)
            {
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
                    TempData[ErrorMessage] = "Deletion cannot be performed. There are existing associated records.";
                }
                else
                {
                    // Handle other exceptions
                    TempData[ErrorMessage] = "An error occurred while deleting the category.";
                    return BadRequest();
                }
            }

            return RedirectToAction("All");
        }
    }
}
