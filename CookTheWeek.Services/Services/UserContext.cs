namespace CookTheWeek.Services.Data.Services
{
    using CookTheWeek.Services.Data.Services.Interfaces;
    public class UserContext : IUserContext
    {
        public Guid UserId { get; set; } = Guid.Empty;
        public bool IsAdmin { get; set; } = false;
    }
}
