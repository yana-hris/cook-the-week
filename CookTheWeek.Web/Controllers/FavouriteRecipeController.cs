namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
   
    using CookTheWeek.Services.Data.Models.FavouriteRecipe;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Common;

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
             OperationResult result = await favouriteRecipeService.TryToggleLikesAsync(model);

            if (result.Succeeded)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "An unexpected error occurred." 
                });
            }
        }
    }
}
