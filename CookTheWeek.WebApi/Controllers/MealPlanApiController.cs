namespace CookTheWeek.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using CookTheWeek.Services.Data.Interfaces;
    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Web.ViewModels.Meal;
    using CookTheWeek.Web.ViewModels.MealPlan;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Common.Exceptions;

    [Route("api/mealplan")]
    [ApiController]
    public class MealPlanApiController : ControllerBase
    {
        private readonly IRecipeService recipeService;
        private readonly IUserRepository userRepository;
        private readonly IUserService userService;
        private readonly ILogger<MealPlanApiController> logger;

        public MealPlanApiController(IRecipeService recipeService,
            IUserRepository userRepository,
            IUserService userService,
            ILogger<MealPlanApiController> logger)
        {
            this.recipeService = recipeService;
            this.userRepository = userRepository;
            this.userService = userService;
            this.logger = logger;
        }

        [HttpPost]
        [Route("getMealPlanData")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MealPlanAddFormModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMealPlanData([FromBody] MealPlanServiceModel model)
        {
            if (!ModelState.IsValid)
            {
                logger.LogWarning($"Invalid model received!");
                return BadRequest(ModelState);
            }
            try
            {
                var user = await this.userRepository.GetUserByIdAsync(model.UserId);
            }
            catch (RecordNotFoundException ex)
            {
                logger.LogError(ex.Message, ex.StackTrace);
                return NotFound();
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

            return Ok(mealPlanModel);

        }
    }
}
