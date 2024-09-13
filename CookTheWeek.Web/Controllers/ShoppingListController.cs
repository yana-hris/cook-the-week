namespace CookTheWeek.Web.Controllers
{
    
    using Microsoft.AspNetCore.Mvc;
    using Rotativa.AspNetCore;

    using Services.Data.Interfaces;
    using ViewModels.ShoppingList;

    
    public class ShoppingListController : BaseController
    {
        private readonly IShoppingListService shoppingListService;
        private readonly IMealPlanService mealPlanService;
        private readonly ILogger<ShoppingListController> logger;

        // Inject IWebHostEnvironment in your controller constructor
        private readonly IWebHostEnvironment hostingEnvironment;

        public ShoppingListController(IShoppingListService shoppingListService,
            ILogger<ShoppingListController> logger,
            IMealPlanService mealPlanService,
            IWebHostEnvironment hostingEnvironment)
        {
            this.shoppingListService = shoppingListService;
            this.logger = logger;
            this.mealPlanService = mealPlanService;
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Generate(string id, string returnUrl = null)
        {
            ShoppingListViewModel model = await this.shoppingListService.GetByMealPlanIdAsync(id);
            ViewBag.ReturnUrl = returnUrl;

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
                logger.LogError($"Meal Plan with id {id} does not exist in the DB.");
                return BadRequest();
            }

            try
            {
                ShoppingListViewModel model = await this.shoppingListService.GetByMealPlanIdAsync(id);
                string contentRootPath = hostingEnvironment.ContentRootPath;

                // Render the partial view to HTML
                return new ViewAsPdf("GeneratePdf", model)
                {
                    FileName = $"Shopping_list_{model.Title}_{DateTime.Today.ToString("dd-MM-yyyy")}.pdf",
                    CustomSwitches = "--print-media-type",
                    PageWidth = 210,
                    PageHeight = 297
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
