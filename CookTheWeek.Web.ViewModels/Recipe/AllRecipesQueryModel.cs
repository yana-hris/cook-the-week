namespace CookTheWeek.Web.ViewModels.Recipe
{
    
    using Enums;

    using static Common.GeneralApplicationConstants;

    public class AllRecipesQueryModel
    {        
        public string? Category { get; set; }
        
        public string? SearchString { get; set; }
        
        public RecipeSorting RecipeSorting { get; set; }

        public int CurrentPage { get; set; }

        public int RecipesPerPage { get; set; }

        public int TotalRecipes { get; set; }

        
    }
}
