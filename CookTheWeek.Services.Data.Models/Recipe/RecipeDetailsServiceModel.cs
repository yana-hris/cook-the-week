namespace CookTheWeek.Services.Data.Models.Recipe
{
    using CookTheWeek.Data.Models;
    public class RecipeDetailsServiceModel
    {
        public Recipe Recipe { get; set; } 
        public int LikesCount { get; set; }
        public int CookedCount { get; set; } 
        public bool IsLikedByUser { get; set; }
        public bool IsInActiveMealPlanForCurrentUser { get; set; }
    }
}
