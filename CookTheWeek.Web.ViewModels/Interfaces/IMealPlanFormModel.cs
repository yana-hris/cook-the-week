namespace CookTheWeek.Web.ViewModels.Interfaces
{
    using CookTheWeek.Web.ViewModels.Meal;

    public interface IMealPlanFormModel
    {
        string Name { get;set; }
        DateTime StartDate { get; set; }
        IList<MealAddFormModel> Meals { get; set; }
    }
}
