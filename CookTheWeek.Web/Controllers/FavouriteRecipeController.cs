namespace CookTheWeek.Web.Controllers
{
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Services.Data.Models.FavouriteRecipe;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class FavouriteRecipeController : BaseController
    {
        private readonly IFavouriteRecipeService favouriteRecipeService;
        public FavouriteRecipeController(ILogger<FavouriteRecipeController> logger,
            IFavouriteRecipeService favouriteRecipeService) 
            : base(logger)
        {
            this.favouriteRecipeService = favouriteRecipeService;
        }

        [HttpPost]
        public async Task<IActionResult> ToggleFavourites([FromBody]FavouriteRecipeServiceModel model)
        {
            try
            {
                await favouriteRecipeService.TryToggleLikesAsync(model);
                return Json(new { success = true });
            }
            catch (ArgumentNullException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch (RecordNotFoundException ex)
            {
                return Json(new { success = false, message = "Record not found" });
            }
            catch (UnauthorizedUserException ex)
            {
                return Json(new { success = false, message = "Unauthorized" });
            }
            catch (Exception ex)
            {
                logger.LogError($"Uncaught exception: {ex.Message}, StackTrace: {ex.StackTrace}");
                return Json(new { success = false, message = "An unexpected error occurred." });
            }
        }
    }
}
