namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using CookTheWeek.Web.ViewModels.Recipe;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Services.Data.Factories;
    using CookTheWeek.Common.Exceptions;

    using static Common.NotificationMessagesConstants;

    public class RecipeAdminController : BaseAdminController
    {
        private readonly IUserService userService;
        private readonly IRecipeViewModelFactory viewModelFactory;

        public RecipeAdminController(IUserService userService,
            IRecipeViewModelFactory viewModelFactory,
            ILogger<RecipeAdminController> logger) 
        : base(logger)
        {
            this.viewModelFactory = viewModelFactory;
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> All([FromQuery] AllRecipesQueryModel queryModel)
        {
            var model = new AllRecipesFilteredAndPagedViewModel();

            try
            {
                model = await viewModelFactory.CreateAllRecipesViewModelAsync(queryModel, false);

                SetViewData("Admin All Recipes", Request.Path + Request.QueryString, null);
                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex.StackTrace);
                return HandleException(ex, nameof(All), nameof(AllRecipesFilteredAndPagedViewModel), null);
            }
        }
        
        [HttpGet]
        public IActionResult Add()
        {
            return Redirect(Url.Action("Add", "Recipe", new { area = "" }));
        }
    }
}
