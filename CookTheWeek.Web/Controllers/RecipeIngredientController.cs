namespace CookTheWeek.Web.Controllers
{
    using CookTheWeek.Services.Data.Models.RecipeIngredient;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    public class RecipeIngredientController : BaseController
    {
        private readonly IIngredientService ingredientService;
        public RecipeIngredientController(ILogger<RecipeIngredientController> logger,
            IIngredientService ingredientService) 
            : base(logger)
        {
            this.ingredientService = ingredientService;
        }

        [HttpGet]
        public async Task<IActionResult> RenderSuggestions(string input)
        {
            try
            {
                var serviceModel = await ingredientService.GenerateIngredientSuggestionsAsync(input);

                return Ok(serviceModel);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something happened and recipe ingredient suggestions loading failed. Error message: {ex.Message}. Error tacktrace: {ex.StackTrace}");
                return BadRequest();
            }
        }
    }
}
