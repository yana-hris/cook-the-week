namespace CookTheWeek.WebApi.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using CookTheWeek.Services.Data.Models.FavouriteRecipe;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Services.Data.Services.Interfaces;

    [Route("api/favouriteRecipe")]
    [ApiController]
    public class FavouriteRecipeApiController : ControllerBase
    {
        
        private readonly IFavouriteRecipeService favouriteRecipeService;
        private readonly ILogger<FavouriteRecipeApiController> logger;

        public FavouriteRecipeApiController(
                    IFavouriteRecipeService favouriteRecipeService,
                    ILogger<FavouriteRecipeApiController> logger)
        {
            this.favouriteRecipeService = favouriteRecipeService;
            this.logger = logger;   
        }

        [HttpPost]
        [Route("toggleFavourites")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ToggleFavourites([FromBody]FavouriteRecipeServiceModel model)
        {
            try
            {
                await favouriteRecipeService.TryToggleLikes(model);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (RecordNotFoundException ex)
            {
                return NotFound(ex);
            }
            catch (UnauthorizedUserException ex)
            {
                return Unauthorized(ex);
            }
            catch(Exception ex)
            {
                logger.LogError($"The following uncaught exception occured: {ex.Message}, Error Stack Trace: {ex.StackTrace}");
                return StatusCode(500, "An unexpected error occured.");
            }
        }
    }
}
