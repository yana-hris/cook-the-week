namespace CookTheWeek.Services.Data.Interfaces
{
    using CookTheWeek.Web.ViewModels.User;

    public interface IUserService
    {
        Task<ICollection<UserViewModel>> AllAsync();
        Task<bool> ExistsByIdAsync(string id);
    }
}
