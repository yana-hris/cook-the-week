namespace CookTheWeek.Web.ViewModels.Admin.Enums
{
    using System.ComponentModel;

    public enum UserSorting
    {
        [Description("A-Z")]
        NameAscending = 0,
        [Description("Z-A")]
        NameDescending = 1,
        [Description("Most Recent")]
        Newest = 2,
        [Description("Oldest")]
        Oldest = 3,
        [Description("Highest No. Meal Plans")]
        MostMealPlans = 4,
        [Description("Highest No. Recipes")]
        MostRecipes = 5
    }
}
