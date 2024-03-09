namespace CookTheWeek.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Models.RecipeIngredient;
    using Services.Interfaces;

    
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
        /// Get suggestion list for word suggestion, based on an input string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Returns a suggestion word list.</returns>
        /// <response code="200">Returns the suggestion list as array</response>
        /// <response code="400">Unsuccessful</response>
        [HttpGet]
        [Route("api/recipeingredient/suggestions")]
        [ProducesResponseType<IEnumerable<RecipeIngredientServiceModel>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Suggesions(string input)
        {
            try
            {
                IEnumerable<RecipeIngredientServiceModel> serviceModel =
                    await ingredientService.GetIngredientSuggestionsAsync(input);

                return Ok(serviceModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("api/recipeingredient/getallingredients")]
        [ProducesResponseType<IEnumerable<RecipeIngredientServiceModel>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllIngredients()
        {
            try
            {
                IEnumerable<RecipeIngredientServiceModel> serviceModel =
                    await ingredientService.GetAllIngredientsAsync();

                return Ok(serviceModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
