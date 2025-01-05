namespace CookTheWeek.Web.ViewModels.Interfaces
{
    
    public interface ISupplyItemModel
    {
        string Name { get; set; }
        string Qty { get; set; }
        string Measure { get; set; }
        string? Note { get; set; }
    }
}
