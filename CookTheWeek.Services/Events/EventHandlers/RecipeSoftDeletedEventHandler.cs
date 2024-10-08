namespace CookTheWeek.Services.Data.Events.EventHandlers
{
    using CookTheWeek.Services.Data.Services.Interfaces;

    public class RecipeSoftDeletedEventHandler : IRecipeSoftDeletedEventHandler
    {
        private readonly IFavouriteRecipeService favouriteRecipeService;
        private readonly IMealService mealService;
        private readonly IRecipeIngredientService recipeIngredientService;
        private readonly IStepService stepService;

        public RecipeSoftDeletedEventHandler(IStepService stepService,
            IFavouriteRecipeService favouriteRecipeService,
            IMealService mealService,
            IRecipeIngredientService recipeIngredientService)
        {
            this.favouriteRecipeService = favouriteRecipeService;
            this.mealService = mealService;
            this.recipeIngredientService = recipeIngredientService;
            this.stepService = stepService;
        }

        public async Task HandleAsync(RecipeSoftDeletedEvent domainEvent)
        {
            // Handle erasing related meal plans and ingredients when a recipe is soft-deleted
            string recipeId = domainEvent.RecipeId.ToString();

            // Delete all relevant recipe Steps, Ingredients and Meals as soft delete will not cascade and delete any connected entities
            await stepService.SoftDeleteAllByRecipeIdAsync(recipeId);
            await recipeIngredientService.SoftDeleteAllByRecipeIdAsync(recipeId);
            await favouriteRecipeService.DeleteAllRecipeLikesAsync(recipeId);
            await mealService.DeleteByRecipeIdAsync(recipeId);
        }
    }
}
