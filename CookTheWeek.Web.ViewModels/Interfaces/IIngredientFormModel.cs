namespace CookTheWeek.Web.ViewModels.Interfaces
{
    
    public interface IIngredientFormModel
    {
        string Name { get; set; }
        int CategoryId { get; set; }
        ICollection<SelectViewModel> Categories { get; set; }
    }
}
