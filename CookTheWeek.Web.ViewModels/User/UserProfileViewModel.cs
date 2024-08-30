namespace CookTheWeek.Web.ViewModels.User
{
    using AngleSharp.Css.Dom;

    public class UserProfileViewModel
    {
        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public bool HasPassword { get; set; }
    }
}
