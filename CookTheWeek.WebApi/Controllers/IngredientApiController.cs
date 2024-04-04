namespace CookTheWeek.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using CookTheWeek.Services.Interfaces;
    using CookTheWeek.Services.Data.Models.Ingredient;

    /// <summary>
    /// Servicing the Ingredients Section in Admin Area
    /// </summary>
    [ApiController]
    [Route("api/ingredient")]
    [Produces("application/json")]
    public class IngredientApiController : ControllerBase
    {
        // TODO: check if will be usedat all
        private readonly IIngredientService ingredientService;

        public IngredientApiController(IIngredientService ingredientService)
        {
            this.ingredientService = ingredientService;
        }

        //[HttpGet]
        //[Route("all")]
        //[ProducesResponseType<IEnumerable<IngredientServiceModel>>(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> GetAllIngredients()
        //{
        //    try
        //    {
        //        IEnumerable<IngredientServiceModel> serviceModel =
        //            await ingredientService.AllFilteredAndSortedAsync();

        //        return Ok(serviceModel);
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest();
        //    }
        //}
    }
}
