namespace CookTheWeek.Web.ViewModels.Recipe.Enums
{
    using System.ComponentModel;

    public enum RecipeSorting
    {
        [Description("Newest")]
        Newest = 0,
        [Description("Oldest")]
        Oldest = 1,
        [Description("Quickest")]
        CookingTimeAscending = 2,
        [Description("Slowest")]
        CookingTimeDescending = 3,
        [Description("Easiest")]
        DifficultyLevelAscending = 4,
        [Description("Hardest")]
        DifficultyLevelDescending = 5,
        [Description("Most Popular")]
        ViewsDescending = 6,
    }
}
