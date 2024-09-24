﻿namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using CookTheWeek.Services.Data.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Services.Interfaces;
    using Web.ViewModels.Admin.MealPlanAdmin;

    public class MealPlanAdminController : BaseAdminController
    {
        private readonly IMealPlanService mealPlanService;

        public MealPlanAdminController(IMealPlanService mealPlanService)
        {
            this.mealPlanService = mealPlanService;
        }
        public async Task<IActionResult> AllActive()
        {
            ICollection<MealPlanAllAdminViewModel> allActive = await
                this.mealPlanService.AllActiveAsync();

            ViewBag.ReturnUrl = "/Admin/MealPlanAdmin/AllActive";

            return View(allActive);
        }

        public async Task<IActionResult> AllFinished()
        {
            ICollection<MealPlanAllAdminViewModel> allFinished = await
                this.mealPlanService.AllFinishedAsync();

            ViewBag.ReturnUrl = "/Admin/MealPlanAdmin/AllFinished";

            return View(allFinished);
        }
    }
}
