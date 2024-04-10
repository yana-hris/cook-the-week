namespace CookTheWeek.WebApi.Controllers
{
    using CookTheWeek.Services.Data.Interfaces;
    using CookTheWeek.Services.Data.Models.FavouriteRecipe;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/favouriteRecipe")]
    [ApiController]
    public class FavouriteRecipeApiController : ControllerBase
    {
        private readonly IRecipeService recipeService;
        private readonly IUserService userService;
        private readonly IFavouriteRecipeService favouriteRecipeService;
        public FavouriteRecipeApiController(IRecipeService recipeService,
            IUserService userService,
            IFavouriteRecipeService favouriteRecipeService)
        {
            this.recipeService = recipeService;
            this.userService = userService;
            this.favouriteRecipeService = favouriteRecipeService;
        }

        [HttpPost]
        [Route("toggleFavourites")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ToggleFavourites([FromBody]FavouriteRecipeServiceModel model)
        {
            string userId = model.UserId;
            string recipeId = model.RecipeId;

            bool recipeExists = await this.recipeService
                .ExistsByIdAsync(recipeId);

            bool userExists = await this.userService.ExistsByIdAsync(userId);

            bool isAlreadyAdded = await this.favouriteRecipeService
                .IsFavouriteRecipeForUserByIdAsync(recipeId, userId);

            if (!recipeExists)
            {
                return NotFound();
            }
            if (!userExists)
            {
                return Unauthorized();
            }
            // If the recipe is already in the user`s favourites, we have to remove it (delete entity FavouriteRecipe)
            if (isAlreadyAdded)
            {
                try
                {
                    await this.favouriteRecipeService.RemoveFromFavouritesByUserId(recipeId, userId);
                }
                catch (Exception)
                {
                    return StatusCode(500, "An unexpected error occurred.");
                }

                return Ok();
            }
            // if not, we have to add it (create FavouriteRecipe entity)
            try
            {
                await this.favouriteRecipeService.AddToFavouritesByUserId(recipeId, userId);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }

            return Created();
            
        }
    }
}
