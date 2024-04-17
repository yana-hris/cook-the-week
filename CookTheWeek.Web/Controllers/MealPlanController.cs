namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using CookTheWeek.Services.Data.Interfaces;
    using CookTheWeek.Web.ViewModels.Meal;
    using CookTheWeek.Web.ViewModels.MealPlan;
    using CookTheWeek.Services.Data.Models.MealPlan;
    using System.Text;

    //[Authorize]
    public class MealPlanController : Controller
    {
        private readonly IMealplanService mealplanService;
        private readonly IRecipeService recipeService;
        private readonly ILogger<MealPlanController> logger;

        public MealPlanController(IMealplanService mealplanService, IRecipeService recipeService, ILogger<MealPlanController> logger)
        {
            this.mealplanService = mealplanService;
            this.recipeService = recipeService;
            this.logger = logger;
        }

        //public Task<IActionResult> AllActive()
        //{

        //}

        //public Task<IActionResult> AllFinished()
        //{

        //}
        
        [HttpPost]
        public IActionResult Add([FromBody] MealPlanAddFormModel model)
        {
            
            return View(model);
        }
    }
}
