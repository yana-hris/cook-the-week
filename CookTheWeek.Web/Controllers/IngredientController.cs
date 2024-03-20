namespace CookTheWeek.Web.Controllers
{    
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using CookTheWeek.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Ingredient;
    using static Common.NotificationMessagesConstants;
    using CookTheWeek.Services.Data.Models.Ingredient;
    using CookTheWeek.Web.ViewModels.Ingredient.Enums;

    [Authorize]
    public class IngredientController : Controller
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
        public async Task<IActionResult> All([FromQuery]AllIngredientsQueryModel queryModel)
        {
            AllIngredientsFilteredAndPagedServiceModel serviceModel = await this.ingredientService
                .AllAsync(queryModel);

            queryModel.Ingredients = serviceModel.Ingredients;
            queryModel.TotalIngredients = serviceModel.TotalIngredientsCount;
            queryModel.Categories = await this.categoryService.AllIngredientCategoryNamesAsync();
            queryModel.IngredientSortings = Enum.GetValues(typeof(IngredientSorting))
                .Cast<IngredientSorting>()
                .ToDictionary(rs => (int)rs, rs => rs.ToString());
            
            return View(queryModel);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            IngredientFormViewModel model = new IngredientFormViewModel();
            model.IngredientCategories = await this.categoryService.AllIngredientCategoriesAsync();


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(IngredientFormViewModel model)
        {    
            bool ingredientExists = await this.ingredientService.ExistsByNameAsync(model.Name);
            bool categoryExists = await this.categoryService.IngredientCategoryExistsByIdAsync(model.IngredientCategoryId);

            if(ingredientExists)
            {
                ModelState.AddModelError(nameof(model.Name), $"Ingredient with name \"{model.Name}\" already exists!");
            }

            if(!categoryExists)
            {
                ModelState.AddModelError(nameof(model.IngredientCategoryId), $"Invalid Ingredient Category: {model.IngredientCategoryId}");
            }

            if(ModelState.IsValid)
            {
                try
                {
                    await this.ingredientService.AddIngredientAsync(model);
                    TempData[SuccessMessage] = $"Ingredient \"{model.Name}\" added successfully!";
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception)
                {
                    TempData[ErrorMessage] = $"Ingredient unsucessfully added! Please try again later or contact administrator!";
                }
            }

            model.IngredientCategories = await this.categoryService.AllIngredientCategoriesAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            bool exists = await this.ingredientService.ExistsByIdAsync(id);

            if(!exists)
            {
                //TODO: Check app logic
                TempData[ErrorMessage] = "Invalid ingredient id!";
                return BadRequest();
            }

            IngredientEditViewModel? model = await this.ingredientService.GetByIdAsync(id)!;
            model.Categories = await this.categoryService.AllIngredientCategoriesAsync();

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(IngredientEditViewModel model)
        {
            bool nameAlreadyExists = await this.ingredientService.ExistsByNameAsync(model.Name);
            bool categoryIsValid = await this.categoryService.IngredientCategoryExistsByIdAsync(model.CategoryId);

            if (nameAlreadyExists)
            {
                ModelState.AddModelError(nameof(model.Name), $"Ingredient with name \"{model.Name}\" already exists!");
            }

            if (!categoryIsValid)
            {
                ModelState.AddModelError(nameof(model.Categories), $"Invalid Ingredient Category: {model.CategoryId}");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await this.ingredientService.EditAsync(model);
                    TempData[SuccessMessage] = $"Ingredient \"{model.Name}\" added successfully!";
                    return RedirectToAction("All");
                }
                catch (Exception)
                {
                    TempData[ErrorMessage] = $"Ingredient unsucessfully edited! Please try again later or contact administrator!";
                }
            }

            model.Categories = await this.categoryService.AllIngredientCategoriesAsync();
            return View(model);
        }
    }
}
