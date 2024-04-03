namespace CookTheWeek.Services.Data.Interfaces
{
    using System.Threading.Tasks;

    public interface IMealplanService
    {
        Task<int> AllActiveCountAsync();
    }
}
