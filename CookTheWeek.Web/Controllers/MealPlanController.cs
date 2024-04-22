namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Infrastructure.Extensions;
    using Services.Data.Interfaces;
    using Services.Data.Models.MealPlan;
    using ViewModels.Meal;
    using ViewModels.MealPlan;

    [Authorize]
    public class MealPlanController : Controller
    {
        private readonly IMealplanService mealplanService;
        private readonly IUserService userService;
        private readonly IRecipeService recipeService;
        private readonly ILogger<MealPlanController> logger;

        public MealPlanController(IMealplanService mealplanService,
            IRecipeService recipeService,
            IUserService userService,
            ILogger<MealPlanController> logger)
        {
            this.mealplanService = mealplanService;
            this.recipeService = recipeService;
            this.userService = userService;
            this.logger = logger;
        }

        [HttpPost]
        [Consumes("application/json")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> CreateMealPlanModel([FromBody]MealPlanServiceModel model)
        {
            if (!ModelState.IsValid)
            {
                logger.LogWarning($"Invalid model received!");
                return BadRequest();
            }

            string userId = model.UserId;
            bool userExists = await this.userService.ExistsByIdAsync(userId);

            if (!userExists || userId != User.GetId())
            {
                ModelState.AddModelError(nameof(model.UserId), "Invalid User Id!");
                logger.LogError($"User with ID {model.UserId} does not exist.");
                return BadRequest(ModelState);
            }

            ICollection<MealServiceModel> recipes = model.Meals;
            MealPlanAddFormModel mealPlanModel = new MealPlanAddFormModel();
            mealPlanModel.Name = "Your Meal Plan";

            foreach (var recipe in recipes)
            {
                try
                {
                    bool exists = await this.recipeService.ExistsByIdAsync(recipe.RecipeId);

                    if (exists)
                    {
                        MealAddFormModel mealModel = await this.recipeService.GetForMealByIdAsync(recipe.RecipeId);
                        mealPlanModel.Meals.Add(mealModel);
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(recipe.RecipeId), "Ivalid Recipe Id");
                        logger.LogWarning($"Recipe ID {recipe.RecipeId} does not exist.");
                        return BadRequest(ModelState);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred while processing recipe ID {recipe.RecipeId}.");
                    return StatusCode(500, "An unexpected error occurred.");
                }
            }

            try
            {
                return RedirectToAction("Add", "MealPlan", mealPlanModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult Add(MealPlanAddFormModel model)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(MealPlanAddFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await this.mealplanService.AddAsync(model);
            }
            catch (Exception ex)
            {
                logger.LogError($"Meal plan with name {model.Name} was not added to the Database!");
                return BadRequest();
            }
        }
    }
}
