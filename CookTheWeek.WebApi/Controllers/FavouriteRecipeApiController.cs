namespace CookTheWeek.WebApi.Controllers
{
    using CookTheWeek.Services.Data.Interfaces;
    using CookTheWeek.Services.Data.Models.FavouriteRecipe;
    using CookTheWeek.Services.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/favouriteRecipe")]
    [ApiController]
    public class FavouriteRecipeApiController : ControllerBase
    {
        private readonly IRecipeService recipeService;
        private readonly IUserService userService;
        public FavouriteRecipeApiController(IRecipeService recipeService,
            IUserService userService)
        {
            this.recipeService = recipeService;
            this.userService = userService;
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

            bool isAlreadyAdded = await this.recipeService
                .IsFavouriteRecipeForUserByIdAsync(recipeId, userId);

            if (!recipeExists)
            {
                return NotFound();
            }
            if (!userExists)
            {
                return Unauthorized();
            }
            // If the recipe is already in the user`s favourites, we have to remove it (delete entity)
            if (isAlreadyAdded)
            {
                try
                {
                    await this.recipeService.RemoveFromFavouritesByUserId(recipeId, userId);
                }
                catch (Exception)
                {
                    return StatusCode(500, "An unexpected error occurred.");
                }

                return Ok();
            } 
            // if not, we have to add it (create entity)
            try
            {
                await this.recipeService.AddToFavouritesByUserId(recipeId, userId);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }

            return Created();
            
        }

        //[HttpPost]
        //[Route("remove")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> RemoveFromFavourites([FromBody]FavouriteRecipeServiceModel model)
        //{
        //    string userId = model.UserId;
        //    string recipeId = model.RecipeId;
            
        //    bool recipeExists = await this.recipeService
        //        .ExistsByIdAsync(recipeId);

        //    bool userExists = await this.userService.ExistsByIdAsync(userId);

        //    bool isAlreadyAdded = await this.recipeService
        //        .IsFavouriteRecipeForUserByIdAsync(recipeId, userId);

        //    if (!recipeExists)
        //    {
        //        return NotFound();
        //    }
        //    if(!userExists)
        //    {
        //        return Unauthorized();
        //    }
        //    if(!isAlreadyAdded)
        //    {
        //        return Forbid();
        //    }
            

        //    return Ok();
        //}
    }
}
