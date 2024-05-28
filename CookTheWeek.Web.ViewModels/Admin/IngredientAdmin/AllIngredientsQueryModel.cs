namespace CookTheWeek.Web.ViewModels.Admin.IngredientAdmin
{
    using System.ComponentModel.DataAnnotations;

    using Enums;
    using static Common.GeneralApplicationConstants;

    public class AllIngredientsQueryModel
    {
        public AllIngredientsQueryModel()
        {
            this.CurrentPage = DefaultPage;
            this.IngredientsPerPage = DefaultIngredientsPerPage;

            this.Categories = new HashSet<string>();
            this.Ingredients = new HashSet<IngredientAllViewModel>();
        }

        public string? Category { get; set; }

        [Display(Name = "Search for..")]
        public string? SearchString { get; set; }

        [Display(Name = "Sort by")]
        public  IngredientSorting IngredientSorting { get; set; }
        public int CurrentPage { get; set; }

        public int IngredientsPerPage { get; set; }

        public int TotalIngredients { get; set; }

        public ICollection<string> Categories { get; set; } = null!;

        public IDictionary<int, string> IngredientSortings { get; set; } = null!;

        public ICollection<IngredientAllViewModel> Ingredients { get; set; }
    }
}
