namespace CookTheWeek.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Models.RecipeIngredient;
    using Services.Interfaces;

    /// <summary>
    /// Servicing the Recipe Ingredients section in Add Recipe View
    /// </summary>
    [ApiController]
    [Route("api/recipeingredient")]
    [Produces("application/json")]
    public class RecipeIngredientApiController : ControllerBase
    {
        private readonly IIngredientService ingredientService;

        
        public RecipeIngredientApiController(IIngredientService ingredientService)
        {
            this.ingredientService = ingredientService;
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
                    await ingredientService.suggestionsAsync(input);

                return Ok(serviceModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
    }
}
