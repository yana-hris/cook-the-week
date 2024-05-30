namespace CookTheWeek.WebApi.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using CookTheWeek.Services.Data.Interfaces;
    using CookTheWeek.Services.Data.Models.FavouriteRecipe;

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
            if (!recipeExists)
            {
                return NotFound();
            }

            bool userExists = await this.userService.ExistsByIdAsync(userId);
            if (!userExists)
            {
                return Unauthorized();
            }

            // If the recipe is already in the user`s favourites, we have to remove it (delete entity FavouriteRecipe)
            bool isAlreadyAdded = await this.favouriteRecipeService
                .ExistsByUserIdAsync(recipeId, userId);
            try
            {
                if (isAlreadyAdded)
                {
                    await this.favouriteRecipeService.RemoveByUserIdAsync(recipeId, userId);
                    return Ok();
                }
                else
                {
                    await this.favouriteRecipeService.AddByUserIdAsync(recipeId, userId);
                    return Ok();
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occured.");
            }           
            
        }
    }
}
