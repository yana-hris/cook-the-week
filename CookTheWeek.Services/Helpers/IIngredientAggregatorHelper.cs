namespace CookTheWeek.Services.Data.Helpers
{
    using CookTheWeek.Services.Data.Models.SupplyItem;
    using CookTheWeek.Web.ViewModels.Interfaces;

    public interface IIngredientAggregatorHelper
    {
        IEnumerable<ISupplyItemListModel<T>> AggregateIngredientsByCategory<T>(
        List<SupplyItemServiceModel> ingredients,
        IEnumerable<ISelectViewModel> measures,
        IEnumerable<ISelectViewModel> specifications,
        Dictionary<string, int[]> categoryDictionary)
            where T : ISupplyItemModel, new();
    }
}
