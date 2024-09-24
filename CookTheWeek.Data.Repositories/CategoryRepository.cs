namespace CookTheWeek.Data.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data.Models.Interfaces;

    public class CategoryRepository<TCategory> : ICategoryRepository<TCategory> 
        where TCategory : class, ICategory, new()
    {
        private readonly CookTheWeekDbContext dbContext;

        public CategoryRepository(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Gets a queryable collection of all categories of type T, which can be used for filtering and sorting and can be materialized with any of the Async methods
        /// </summary>
        /// <returns>A collection of IQueryable of TCategory</returns>
        public IQueryable<TCategory> GetAllQuery()
        {
            return dbContext.Set<TCategory>().AsNoTracking();
        }

        /// <summary>
        /// Adds a TCategory to the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>the new entity Id (int) or 0</returns>
        public async Task<int?> AddAsync(TCategory entity)
        {
            try
            {
                await dbContext.Set<TCategory>().AddAsync(entity);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return 0;
            }

            return entity.Id;   
        }

        /// <summary>
        /// Edits the current TCategory and persists changes in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task EditAsync(TCategory entity)
        {
            dbContext.Set<TCategory>().Update(entity);
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Finds a TCategory by id and deletes it from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteByIdAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                dbContext.Set<TCategory>().Remove(entity);
                await dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Returns a single TCategory by id if it exists in the database. If not, returns null
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TCategory or null</returns>
        public async Task<TCategory?> GetByIdAsync(int id)
        {
            return await dbContext.Set<TCategory>().
                FirstOrDefaultAsync(c => c.Id == id);
        }

        /// <summary>
        /// Returns a flag if a TCategory exists in the database by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true or false</returns>
        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await dbContext.Set<TCategory>().AnyAsync(e => e.Id == id);
        }
    }

}
