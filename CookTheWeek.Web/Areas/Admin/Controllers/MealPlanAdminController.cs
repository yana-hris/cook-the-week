namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using CookTheWeek.Services.Data.Factories;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    using Web.ViewModels.Admin.MealPlanAdmin;

    public class MealPlanAdminController : BaseAdminController
    {
        private readonly IViewModelFactory viewModelFactory;

        public MealPlanAdminController(
            IViewModelFactory viewModelFactory,
            ILogger<MealPlanAdminController> logger) : base(logger) 
        {
            this.viewModelFactory = viewModelFactory;
        }

        [HttpGet]
        public async Task<IActionResult> AllActive()
        {
            ICollection<MealPlanAllAdminViewModel> allActive = await
                viewModelFactory.CreateAllActiveMealPlansAdminViewModelAsync();

            ViewBag.ReturnUrl = "/Admin/MealPlanAdmin/AllActive";

            return View(allActive);
        }

        [HttpGet]
        public async Task<IActionResult> AllFinished()
        {
            ICollection<MealPlanAllAdminViewModel> allFinished = await
                viewModelFactory.CreateAllFinishedMealPlansAdminViewModelAsync();

            ViewBag.ReturnUrl = "/Admin/MealPlanAdmin/AllFinished";

            return View(allFinished);
        }
    }
}
