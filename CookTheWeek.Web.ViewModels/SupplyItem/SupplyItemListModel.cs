namespace CookTheWeek.Web.ViewModels.SupplyItem
{
    using CookTheWeek.Web.ViewModels.Interfaces;

    public class SupplyItemListModel<T> : ISupplyItemListModel<T> 
        where T : ISupplyItemModel, new()
    {
        public string Title { get; set; } = null!;

        public ICollection<T> SupplyItems { get; set; } = new List<T>();
    }

}
