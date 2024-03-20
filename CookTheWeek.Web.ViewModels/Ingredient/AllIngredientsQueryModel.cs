namespace CookTheWeek.Web.ViewModels.Ingredient
{
    using CookTheWeek.Web.ViewModels.Ingredient.Enums;
    using System.Security.Cryptography.X509Certificates;
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

        public string? SearchString { get; set; }

        public  IngredientSorting IngredientSorting { get; set; }
        public int CurrentPage { get; set; }

        public int IngredientsPerPage { get; set; }

        public int TotalIngredients { get; set; }

        public ICollection<string> Categories { get; set; }

        public IDictionary<int, string> IngredientSortings { get; set; }

        public ICollection<IngredientAllViewModel> Ingredients { get; set; }
    }
}
