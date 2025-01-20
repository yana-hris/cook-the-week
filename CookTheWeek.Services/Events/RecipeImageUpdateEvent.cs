namespace CookTheWeek.Services.Data.Events
{
    
    public class RecipeImageUpdateEvent
    {
        public Guid RecipeId { get; set; }

        public string ExternalImageUrl { get; set; } = null!;
    }
}
