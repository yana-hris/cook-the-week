namespace CookTheWeek.Web.ViewModels.User
{
    public class UserProfileViewModel
    {
        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public bool HasPassword { get; set; }

        public int? RecipesCount { get; set; }

        public int? MealplansCount { get; set; }
    }
}
