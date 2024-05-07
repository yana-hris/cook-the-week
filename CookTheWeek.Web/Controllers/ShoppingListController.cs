namespace CookTheWeek.Web.Controllers
{
    using CookTheWeek.Services.Data.Interfaces;
    using CookTheWeek.Web.ViewModels.ShoppingList;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ShoppingListController : Controller
    {
        private readonly IShoppingListService shoppingListService;

        public ShoppingListController(IShoppingListService shoppingListService)
        {
            this.shoppingListService = shoppingListService;
        }

        [HttpGet]
        public async Task<IActionResult> Generate(string id)
        {
            ShoppingListViewModel model = await this.shoppingListService.GetByMealPlanId(id);

            return View(model);
        }
    }
}
