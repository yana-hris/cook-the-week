namespace CookTheWeek.Services.Data.Interfaces
{
    using System.Threading.Tasks;
    public interface IUserAdminService
    {
        Task<string[]> AllUsersInRoleIdsAsync(string roleName);
        Task<string[]> AllUsersNotInRoleIdsAsync(string roleName);
    }
}
