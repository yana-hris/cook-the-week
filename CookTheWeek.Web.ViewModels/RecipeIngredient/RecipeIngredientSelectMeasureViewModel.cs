namespace CookTheWeek.Web.ViewModels.RecipeIngredient
{
    using CookTheWeek.Web.ViewModels.Interfaces;

    public class RecipeIngredientSelectMeasureViewModel : ICategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
