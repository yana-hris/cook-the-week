namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using CookTheWeek.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Ingredient;
    using static Common.NotificationMessagesConstants;
    using CookTheWeek.Services.Data.Models.Ingredient;
    using CookTheWeek.Web.ViewModels.Ingredient.Enums;
    
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
        public async Task<IActionResult> Add(IngredientFormViewModel model)
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
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception)
                {
                    TempData[ErrorMessage] = $"Ingredient unsucessfully added! Please try again later or contact administrator!";
                }
            }

            model.IngredientCategories = await categoryService.AllIngredientCategoriesAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // We assume the admin will not perform parameter tampering, so no checks are needed
            IngredientEditViewModel model = await ingredientService.GetForEditByIdAsync(id);
            model.Categories = await categoryService.AllIngredientCategoriesAsync();

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IngredientEditViewModel model)
        {
            bool nameAlreadyExists = await ingredientService.ExistsByNameAsync(model.Name);

            if (nameAlreadyExists)
            {
                ModelState.AddModelError(nameof(model.Name), $"Ingredient with name \"{model.Name}\" already exists!");
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
            try
            {
                await this.ingredientService.DeleteById(id);
                TempData[SuccessMessage] = "Ingredient successfully deleted!";
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return RedirectToAction("All");
        }
    }
}
