namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using CookTheWeek.Services.Data.Interfaces;
    using CookTheWeek.Web.ViewModels.MealPlan;

    public class MealPlanAdminController : BaseAdminController
    {
        private readonly IMealPlanService mealPlanService;

        public MealPlanAdminController(IMealPlanService mealPlanService)
        {
            this.mealPlanService = mealPlanService;
        }
        public async Task<IActionResult> AllActive()
        {
            ICollection<MealPlanAllViewModel> allActive = await
                this.mealPlanService.AllActiveAsync();

            return View(allActive);
        }

        public async Task<IActionResult> AllFinished()
        {
            ICollection<MealPlanAllViewModel> allFinished = await
                this.mealPlanService.AllFinishedAsync();

            return View(allFinished);
        }
    }
}
