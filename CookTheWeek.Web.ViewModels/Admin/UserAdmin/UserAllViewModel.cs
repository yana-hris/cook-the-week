namespace CookTheWeek.Web.ViewModels.Admin.UserAdmin
{
    public class UserAllViewModel
    {
        public string Id { get; set; } = null!;

        public string Username { get; set; } = null!;

        public bool HasPassword { get; set; }

        public string Email { get; set; } = null!;

        public int? TotalRecipes { get; set; }

        public int? TotalMealPlans { get; set; }
    }
}
