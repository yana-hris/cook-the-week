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

        /// <inheritdoc/> 
        public IQueryable<TCategory> GetAllQuery()
        {
            return dbContext.Set<TCategory>().AsNoTracking().AsQueryable();
        }

        /// <inheritdoc/> 
        public async Task AddAsync(TCategory entity)
        {
            await dbContext.Set<TCategory>().AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/> 
        public async Task UpdateAsync(TCategory entity)
        {
            dbContext.Set<TCategory>().Update(entity);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/> 
        public async Task DeleteByIdAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                dbContext.Set<TCategory>().Remove(entity);
                await dbContext.SaveChangesAsync();
            }
        }

        /// <inheritdoc/> 
        public async Task<TCategory?> GetByIdAsync(int id)
        {
            return await dbContext.Set<TCategory>().
                FirstOrDefaultAsync(c => c.Id == id);
        }

        /// <inheritdoc/> 
        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await dbContext.Set<TCategory>().AnyAsync(e => e.Id == id);
        }

        /// <inheritdoc/> 
        public async Task<bool> HasDependenciesAsync<TDependency>(int categoryId) 
            where TDependency : class
        {
            return await dbContext
                .Set<TDependency>()
                .AnyAsync(d => EF.Property<int>(d, "CategoryId") == categoryId);
        }

        /// <inheritdoc/> 
        public async Task<int?> GetIdByNameAsync(string name)
        {
            return await dbContext
                .Set<TCategory>()
                .Where(c => c.Name.ToLower() == name.ToLower())
                .Select(c => c.Id)
                .FirstOrDefaultAsync();
        }
    }

}
