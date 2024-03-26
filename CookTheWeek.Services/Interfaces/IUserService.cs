namespace CookTheWeek.Services.Data.Interfaces
{
    
    public interface IUserService
    {
        Task<bool> ExistsByIdAsync(string id);
    }
}
