namespace CookTheWeek.Web.Controllers
{
    
    using Microsoft.AspNetCore.Mvc;

    using CookTheWeek.Services.Data.Services.Interfaces;

    public class RecipeIngredientController : BaseController
    {
        private readonly IIngredientService ingredientService;
        public RecipeIngredientController(ILogger<RecipeIngredientController> logger,
            IIngredientService ingredientService) 
            : base(logger)
        {
            this.ingredientService = ingredientService;
        }

        /// <summary>
        /// Provides ingredient suggestions based on the user's input string.
        /// </summary>
        /// <param name="input">The user input to base suggestions on.</param>
        /// <returns>Returns a JSON array of suggestion words.</returns>
        [HttpGet]
        public async Task<IActionResult> RenderSuggestions(string input)
        {
            try
            {
                var serviceModel = await ingredientService.GenerateIngredientSuggestionsAsync(input);

                return Ok(serviceModel);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something happened and recipe ingredient suggestions loading failed. Error message: {ex.Message}. Error tacktrace: {ex.StackTrace}");
                return BadRequest();
            }
        }
    }
}
