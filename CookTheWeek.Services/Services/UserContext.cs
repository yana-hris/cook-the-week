namespace CookTheWeek.Services.Data.Services
{
    using CookTheWeek.Services.Data.Services.Interfaces;
    public class UserContext : IUserContext
    {
        public string UserId { get; set; }
    }
}
