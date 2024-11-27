namespace CookTheWeek.Services.Data.Models.Recipe
{
    
    public class RecipeAllMineServiceModel
    {
        public List<string> LikedRecipeIds { get; set; } = new();
        public List<string> AddedRecipeIds { get; set; } = new();
    }
}
