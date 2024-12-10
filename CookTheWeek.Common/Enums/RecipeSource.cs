namespace CookTheWeek.Common.Enums
{
    using System.ComponentModel;

    public enum RecipeSource
    {
        [Description("Site Recipes")]
        Site = 1,
        [Description("User Recipes")]
        User = 2
    }
}
