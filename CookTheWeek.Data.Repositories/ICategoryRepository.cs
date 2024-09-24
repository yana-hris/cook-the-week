
namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models.Interfaces;

    public interface ICategoryRepository<T> where T : ICategory
    {
        IQueryable<ICategory> GetAllQuery();
    }
}
