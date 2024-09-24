namespace CookTheWeek.WebApi.Controllers
{
    using CookTheWeek.Services.Data.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Services.Interfaces;
    using Services.Data.Models.RecipeIngredient;

    /// <summary>
    /// Servicing the Recipe Ingredients section in Add Recipe View
    /// </summary>
    [ApiController]
    [Route("api/recipeingredient")]
    [Produces("application/json")]
    public class RecipeIngredientApiController : ControllerBase
    {
        private readonly IIngredientService ingredientService;
        private readonly ILogger<RecipeIngredientApiController> logger;
        
        public RecipeIngredientApiController(IIngredientService ingredientService,
            ILogger<RecipeIngredientApiController> logger)
        {
            this.ingredientService = ingredientService;
            this.logger = logger;
        }
        /// <summary>
        /// Get suggestion list for ingredients to be included in a recipe, based on an input string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Returns a suggestion word list.</returns>
        /// <response code="200">Returns the suggestion list as array</response>
        /// <response code="400">Unsuccessful</response>
        [HttpGet]
        [Route("suggestions")]
        [ProducesResponseType<IEnumerable<RecipeIngredientSuggestionServiceModel>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Suggesions(string input)
        {
            try
            {
                IEnumerable<RecipeIngredientSuggestionServiceModel> serviceModel =
                    await ingredientService.GenerateIngredientSuggestionsAsync(input);

                return Ok(serviceModel);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something happened and recipe ingredient suggestions loading failed. Error message: {ex.Message}. Error tacktrace: {ex.StackTrace}")
                return BadRequest();
            }
        }
        
    }
}
