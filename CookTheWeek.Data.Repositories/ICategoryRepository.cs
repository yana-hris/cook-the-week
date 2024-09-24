
namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models.Interfaces;

    public interface ICategoryRepository<TCategory> where TCategory : class, ICategory
    {
        IQueryable<TCategory> GetAllQuery();
        Task<int?> AddAsync(TCategory entity);
        Task EditAsync(TCategory entity);
        Task DeleteByIdAsync(int id);
        Task<TCategory?> GetByIdAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
    }
}
