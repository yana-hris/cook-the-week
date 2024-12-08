namespace CookTheWeek.Web.ViewModels.Admin.IngredientAdmin.Enums
{
    using System.ComponentModel;

    public enum IngredientSorting
    {
        [Description("A-Z")]
        NameAscending = 0,
        [Description("Z-A")]
        NameDescending = 1,
        [Description("Min ID")]
        IdAscending = 2,
        [Description("Max ID")]
        IdDescending = 3,
    }
}
