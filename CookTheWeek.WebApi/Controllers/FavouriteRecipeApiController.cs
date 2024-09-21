namespace CookTheWeek.WebApi.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using CookTheWeek.Services.Data.Interfaces;
    using CookTheWeek.Services.Data.Models.FavouriteRecipe;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Common.Exceptions;

    [Route("api/favouriteRecipe")]
    [ApiController]
    public class FavouriteRecipeApiController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IRecipeRepository recipeRepository;
        private readonly IFavouriteRecipeRepository favouriteRecipeRepository;
        private readonly ILogger<FavouriteRecipeApiController> logger;

        public FavouriteRecipeApiController(IUserService userService,
            IRecipeRepository recipeRepository,
            IFavouriteRecipeRepository favouriteRecipeRepository,
            ILogger<FavouriteRecipeApiController> logger)
        {
            
            this.userService = userService;
            this.recipeRepository = recipeRepository;
            this.favouriteRecipeRepository = favouriteRecipeRepository;
            this.logger = logger;   
        }

        [HttpPost]
        [Route("toggleFavourites")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ToggleFavourites([FromBody]FavouriteRecipeServiceModel model)
        {
            string userId = model.UserId;
            string recipeId = model.RecipeId;

            if (userId == null)
            {
                logger.LogError("User must be logged in to like and unlike recipes");
                return Unauthorized();
            }

            try
            {
                var recipe = await this.recipeRepository.GetByIdAsync(recipeId);
                bool isAlreadyAdded = await this.favouriteRecipeRepository.GetByIdAsync(userId, recipeId);

                if (isAlreadyAdded)
                {
                    await this.favouriteRecipeRepository.DeleteAsync(userId, recipeId);
                }
                else
                {
                    await this.favouriteRecipeRepository.AddAsync(userId, recipeId);
                }

                return Ok();
            }
            catch (RecordNotFoundException ex)
            {
                logger.LogError($"Recipe with id {recipeId} not found in the database. Error stacktrace: {ex.StackTrace}");
                return NotFound();
            }
            catch(UnauthorizedUserException ex)
            {
                logger.LogError($"User has no authorization rights to like/unline this recipe. Exception stackTrace: {ex.StackTrace}");
                return Unauthorized();
            }
            catch(DataRetrievalException ex)
            {
                logger.LogError($"The following data retrieval exception occured: {ex.Message}, Error Stack Trace: {ex.StackTrace}");
                return StatusCode(500, "An unexpected error occured.");
            }
            catch(Exception ex)
            {
                logger.LogError($"The following uncaught exception occured: {ex.Message}, Error Stack Trace: {ex.StackTrace}");
                return StatusCode(500, "An unexpected error occured.");
            }
           
        }
    }
}
