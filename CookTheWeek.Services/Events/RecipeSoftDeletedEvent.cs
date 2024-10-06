namespace CookTheWeek.Services.Data.Events
{
    /// <summary>
    /// This class simply holds the information that will be passed to the event handler, such as the RecipeId.
    /// </summary>
    public class RecipeSoftDeletedEvent
    {
        public Guid RecipeId { get; }

        public RecipeSoftDeletedEvent(Guid recipeId)
        {
            RecipeId = recipeId;
        }
    }
}
