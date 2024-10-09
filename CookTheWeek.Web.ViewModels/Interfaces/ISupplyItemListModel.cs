namespace CookTheWeek.Web.ViewModels.Interfaces
{    
    public interface ISupplyItemListModel<T> 
        where T : ISupplyItemModel, new()
    {
        string Title { get; set; }

        ICollection<T> SupplyItems { get; set; }
    }
}
