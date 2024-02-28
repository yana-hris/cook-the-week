namespace CookTheWeek.Web.Controllers
{
    using CookTheWeek.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Ingredient;
    using Microsoft.AspNetCore.Mvc;

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
            model.IngredientCategories = await this.categoryService.GetAllIngredientCategoriesAsync();


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(IngredientFormViewModel model)
        {            
            model.IngredientCategories = await this.categoryService.GetAllIngredientCategoriesAsync();
            bool ingredientExists = await this.ingredientService.existsByNameAsync(model.Name);

            if(ingredientExists)
            {
                ModelState.AddModelError(nameof(model.Name), $"Ingredient with name \"{model.Name}\" already exists!");
            }

            if(ModelState.IsValid)
            {
                try
                {
                    await this.ingredientService.AddIngredientAsync(model);
                    TempData["SuccessMessage"] = $"Ingredient \"{model.Name}\" added successfully!";
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = $"Ingredient with \"{model.Name}\" was not added to the database!";
                }
            }
            
            return View(model);
        }
    }
}
