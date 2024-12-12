namespace CookTheWeek.Services.Data.Events.EventHandlers
{
    using CookTheWeek.Services.Data.Services.Interfaces;

    public class RecipeSoftDeletedEventHandler : IRecipeSoftDeletedEventHandler
    {
        private readonly IFavouriteRecipeService favouriteRecipeService;
        private readonly IMealService mealService;
        private readonly IRecipeIngredientService recipeIngredientService;
        private readonly IStepService stepService;
        private readonly IRatingService ratingService;

        public RecipeSoftDeletedEventHandler(IStepService stepService,
            IFavouriteRecipeService favouriteRecipeService,
            IMealService mealService,
            IRatingService ratingService,
            IRecipeIngredientService recipeIngredientService)
        {
            this.favouriteRecipeService = favouriteRecipeService;
            this.mealService = mealService;
            this.recipeIngredientService = recipeIngredientService;
            this.ratingService = ratingService;
            this.stepService = stepService;
        }

        public async Task HandleAsync(RecipeSoftDeletedEvent domainEvent)
        {
            // Handle erasing related meal plans and ingredients when a recipe is soft-deleted
            Guid recipeId = domainEvent.RecipeId;

            // Soft Delete all relevant recipe Steps, Ingredients and Meals 
            await recipeIngredientService.SoftDeleteAllByRecipeIdAsync(recipeId);
            await stepService.SoftDeleteAllByRecipeIdAsync(recipeId);
            await mealService.SoftDeleteAllByRecipeIdAsync(recipeId);
            await favouriteRecipeService.SoftDeleteAllByRecipeIdAsync(recipeId);
            await ratingService.SoftDeleteAllByRecipeIdAsync(recipeId);
        }
    }
}
