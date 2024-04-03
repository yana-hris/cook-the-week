namespace CookTheWeek.Web.Controllers
{
    using CookTheWeek.Services.Data.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class MealPlanController : Controller
    {
        private readonly IMealplanService mealplanService;

        public MealPlanController(IMealplanService mealplanService)
        {
            this.mealplanService = mealplanService;
        }

        //public Task<IActionResult> AllActive()
        //{
            
        //}

        //public Task<IActionResult> AllFinished()
        //{

        //}
    }
}
