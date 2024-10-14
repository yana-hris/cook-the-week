namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using CookTheWeek.Services.Data.Factories;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    using Web.ViewModels.Admin.MealPlanAdmin;

    using static CookTheWeek.Common.GeneralApplicationConstants;

    public class MealPlanAdminController : BaseAdminController
    {
        private readonly IMealPlanViewModelFactory viewModelFactory;

        public MealPlanAdminController(
            IMealPlanViewModelFactory viewModelFactory,
            ILogger<MealPlanAdminController> logger) : base(logger) 
        {
            this.viewModelFactory = viewModelFactory;
        }

        [HttpGet]
        public async Task<IActionResult> AllActive()
        {
            ICollection<MealPlanAllAdminViewModel> allActive = await
                viewModelFactory.CreateAllActiveMealPlansAdminViewModelAsync();

            ViewBag.ReturnUrl = Url.Action("AllActive", "MealPlanAdmin", new { area = AdminAreaName }); //"/Admin/MealPlanAdmin/AllActive";

            return View(allActive);
        }

        [HttpGet]
        public async Task<IActionResult> AllFinished()
        {
            ICollection<MealPlanAllAdminViewModel> allFinished = await
                viewModelFactory.CreateAllFinishedMealPlansAdminViewModelAsync();

            ViewBag.ReturnUrl = ViewBag.ReturnUrl = Url.Action("AllFinished", "MealPlanAdmin", new { area = AdminAreaName });//"/Admin/MealPlanAdmin/AllFinished";

            return View(allFinished);
        }
    }
}
