namespace CookTheWeek.Common.HelperMethods
{
    public static class BadgeColorHelper
    {
        public static string GetBadgeColor(string mealCategoryName)
        {
            return mealCategoryName switch
            {
                "Breakfast" => "bg-breakfast-yellow text-dark",
                "Soup" => "bg-soup-orange",
                "Salad" => "bg-salad-green",
                "Main Dish" => "bg-main-dish-red",
                "Appetizer" => "bg-appetizer-green text-dark",
                "Dessert" => "bg-dessert-pink text-dark",
                _ => "bg-white text-dark border border-dark"
            };
        }
    }
}
