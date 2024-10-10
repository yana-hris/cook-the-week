namespace CookTheWeek.Services.Data.Services
{
    using CookTheWeek.Services.Data.Services.Interfaces;
    public class UserContext : IUserContext
    {
        public string UserId { get; set; } = string.Empty;
        public bool IsAdmin { get; set; } = false;
    }
}
