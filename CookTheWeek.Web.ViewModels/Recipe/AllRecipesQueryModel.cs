namespace CookTheWeek.Web.ViewModels.Recipe
{
   
    public class AllRecipesQueryModel
    {
        // Filters
        public string? SearchString { get; set; }
        public int? MealTypeId { get; set; }        
        public int? MaxPreparationTime { get; set; } // e.g., 30 minutes
        public int? DifficultyLevel { get; set; } // Easy, Medium, Hard
        public List<int>? SelectedTagIds { get; set; } // e.g., Kid-Friendly, Healthy, Autumn, etc.
        
        public int? RecipeSource {  get; set; }

        // Pagination
        public int CurrentPage { get; set; }
        public int RecipesPerPage { get; set; }
        public int TotalResults { get; set; }


        // Sorting
        public int? RecipeSorting { get; set; }
    }
}
