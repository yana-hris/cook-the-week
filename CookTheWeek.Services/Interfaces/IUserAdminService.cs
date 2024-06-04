namespace CookTheWeek.Services.Data.Interfaces
{
    using CookTheWeek.Web.ViewModels.Admin.UserAdmin;
    using System.Threading.Tasks;
    public interface IUserAdminService
    {
        Task<ICollection<UserAllViewModel>> AllAsync();
        Task<int> AllCountAsync();
        Task<string[]> AllUsersInRoleIdsAsync(string roleName);
        Task<string[]> AllUsersNotInRoleIdsAsync(string roleName);
    }
}
