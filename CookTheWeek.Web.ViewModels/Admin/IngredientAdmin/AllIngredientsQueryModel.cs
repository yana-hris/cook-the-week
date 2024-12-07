namespace CookTheWeek.Web.ViewModels.Admin.IngredientAdmin
{
    using System.ComponentModel.DataAnnotations;
    
    using static Common.GeneralApplicationConstants;

    public class AllIngredientsQueryModel
    {
        public AllIngredientsQueryModel()
        {
            this.CurrentPage = DefaultPage;
            this.IngredientsPerPage = DefaultIngredientsPerPage;

            this.Categories = new List<SelectViewModel>();
            this.Ingredients = new HashSet<IngredientAllViewModel>();
        }

        // Filters
        [Display(Name = "Search for..")]
        public string? SearchString { get; set; }
        public int? CategoryId { get; set; }       


        // Pagination
        public int CurrentPage { get; set; }
        public int IngredientsPerPage { get; set; }
        public int TotalResults { get; set; }


        // Sorting

        [Display(Name = "Sort by")]
        public  int? IngredientSorting { get; set; }   
        public ICollection<SelectViewModel> IngredientSortings { get; set; } = null!;


        // Result Set Collection
        public ICollection<IngredientAllViewModel> Ingredients { get; set; }


        // Select Options
        public ICollection<SelectViewModel> Categories { get; set; } = null!;
    }
}
