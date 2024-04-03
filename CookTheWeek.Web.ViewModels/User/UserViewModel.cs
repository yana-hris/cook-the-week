namespace CookTheWeek.Web.ViewModels.User
{
    public class UserViewModel
    {
        public string Id { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string Email { get; set; }

        public int? TotalRecipes { get; set; }

        public int? TotalMealPlans { get; set; }
    }
}
