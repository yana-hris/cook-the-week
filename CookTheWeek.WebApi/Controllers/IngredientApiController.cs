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
        private readonly IIngredientService ingredientService;

        public IngredientApiController(IIngredientService ingredientService)
        {
            this.ingredientService = ingredientService;
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType<IEnumerable<IngredientServiceModel>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllIngredients()
        {
            try
            {
                IEnumerable<IngredientServiceModel> serviceModel =
                    await ingredientService.GetAllIngredientsAsync();

                return Ok(serviceModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //[HttpGet]
        //[Route("allbycategoryid")]
        //[ProducesResponseType<IEnumerable<IngredientServiceModel>>(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> GetAllByCategory(int categoryId)
        //{
        //    try
        //    {
        //        IEnumerable<IngredientServiceModel> serviceModel =
        //            await ingredientService.GetAllByCategoryId(categoryId);

        //        return Ok(serviceModel);
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest();
        //    }
        //}

        //[HttpGet]
        //[Route("existsByName")]
        //[ProducesResponseType<bool>(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> GetAllByCategory(string name)
        //{
        //    try
        //    {
        //        bool result =
        //            await ingredientService.ExistsByNameAsync(name);

        //        return Ok(result);
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest();
        //    }
        //}
    }
}
