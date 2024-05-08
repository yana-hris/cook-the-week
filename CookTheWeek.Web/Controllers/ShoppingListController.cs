﻿namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewEngines;
    using Rotativa.AspNetCore;

    using Services.Data.Interfaces;
    using ViewModels.ShoppingList;

    [Authorize]
    public class ShoppingListController : Controller
    {
        private readonly IShoppingListService shoppingListService;
        private readonly IMealPlanService mealPlanService;
        private readonly ICompositeViewEngine viewEngine;
        private readonly ILogger<ShoppingListController> logger;

        public ShoppingListController(IShoppingListService shoppingListService,
            ICompositeViewEngine viewEngine,
            ILogger<ShoppingListController> logger,
            IMealPlanService mealPlanService)
        {
            this.shoppingListService = shoppingListService;
            this.viewEngine = viewEngine;
            this.logger = logger;
            this.mealPlanService = mealPlanService;
        }

        [HttpGet]
        public async Task<IActionResult> Generate(string id)
        {
            ShoppingListViewModel model = await this.shoppingListService.GetByMealPlanId(id);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GeneratePdf(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                logger.LogError("Model binding not working, string is not parsed from ajax request.");
                return BadRequest();
            }

            bool exists = await this.mealPlanService.ExistsByIdAsync(id);

            if (!exists)
            {
                logger.LogError($"Meal Plan with id {id} does not exist in the DB");
                return BadRequest();
            }

            try
            {
                ShoppingListViewModel model = await this.shoppingListService.GetByMealPlanId(id);
                return new ViewAsPdf("Generate", model)
                {
                    FileName = $"Shopping_list_{model.Title}_{DateTime.Today.ToString("dd-MM-yyyy")}.pdf" 
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error generating PDF.");
                return BadRequest("Error generating PDF."); 
            }
        }
        
    }
}
