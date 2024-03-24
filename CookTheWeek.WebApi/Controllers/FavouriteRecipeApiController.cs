namespace CookTheWeek.WebApi.Controllers
{
    using CookTheWeek.Services.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/favouriteRecipe")]
    [Produces("application/json")]
    [ApiController]
    public class FavouriteRecipeApiController : ControllerBase
    {
        private readonly IRecipeService recipeService;
        public FavouriteRecipeApiController(IRecipeService recipeService)
        {
            this.recipeService = recipeService;
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddToFavourites(string id, string userId)
        {
            bool exists = await this.recipeService
                .ExistsByIdAsync(id);
            bool isAlreadyAdded = await this.recipeService
                .IsFavouriteRecipeForUserByIdAsync(id, userId);

            if (!exists)
            {
                return NotFound();
            }
            if(string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            if(isAlreadyAdded)
            {
                return Forbid();
            }
            
            try
            {
                await this.recipeService.AddToFavouritesByUserId(id, userId);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }           

            return Created();
        }

        [HttpPost]
        [Route("remove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveFromFavourites(string id, string userId)
        {
            bool exists = await this.recipeService
                .ExistsByIdAsync(id);
            bool isAlreadyAdded = await this.recipeService
                .IsFavouriteRecipeForUserByIdAsync(id, userId);

            if (!exists)
            {
                return NotFound();
            }
            if(string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            if(!isAlreadyAdded)
            {
                return Forbid();
            }
            try
            {
                await this.recipeService.RemoveFromFavouritesByUserId(id, userId);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }

            return Ok();
        }
    }
}
