namespace CookTheWeek.Services.Data.Services.Interfaces
{
    public interface IUserContext
    {
        string UserId { get; set; }

        bool IsAdmin { get; set; }
    }
}
