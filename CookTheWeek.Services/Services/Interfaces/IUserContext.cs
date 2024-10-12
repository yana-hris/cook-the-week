namespace CookTheWeek.Services.Data.Services.Interfaces
{
    public interface IUserContext
    {
        Guid UserId { get; set; }

        bool IsAdmin { get; set; }
    }
}
