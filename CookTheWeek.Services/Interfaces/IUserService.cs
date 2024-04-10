namespace CookTheWeek.Services.Data.Interfaces
{
    using CookTheWeek.Web.ViewModels.User;

    public interface IUserService
    {
        Task<ICollection<UserViewModel>> AllAsync();
        Task<int> AllUsersCountAsync();
        Task<bool> ExistsByIdAsync(string id);
        Task<bool> IsOwner(string id, string ownerId);
    }
}
