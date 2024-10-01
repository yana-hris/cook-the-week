namespace CookTheWeek.Web.ViewModels.Interfaces
{
    using CookTheWeek.Web.ViewModels.Category;

    public interface IIngredientFormModel
    {
        string Name { get; set; }
        int CategoryId { get; set; }
        ICollection<IngredientCategorySelectViewModel> Categories { get; set; }
    }
}
