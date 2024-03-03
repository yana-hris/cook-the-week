namespace CookTheWeek.Web.Controllers
{    
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using CookTheWeek.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Ingredient;
    using static Common.NotificationMessagesConstants;

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

        //public IActionResult All()
        //{
        //    return View();
        //}

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
            bool ingredientExists = await this.ingredientService.existsByNameAsync(model.Name);
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
                    TempData[ErrorMessage] = $"Something with the database went wrong and ingredient was not added to the database!";
                }
            }

            model.IngredientCategories = await this.categoryService.AllIngredientCategoriesAsync();
            return View(model);
        }
    }
}
