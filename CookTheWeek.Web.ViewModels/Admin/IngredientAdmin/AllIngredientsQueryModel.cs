namespace CookTheWeek.Web.ViewModels.Admin.IngredientAdmin
{
    using System.ComponentModel.DataAnnotations;    
    
    public class AllIngredientsQueryModel
    {
        public AllIngredientsQueryModel()
        {
            this.Categories = new List<SelectViewModel>();
            this.IngredientSortings = new List<SelectViewModel>();
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
        public ICollection<IngredientAllViewModel> Ingredients { get; set; } = null!;


        // Select Options
        public ICollection<SelectViewModel> Categories { get; set; } = null!;
    }
}
