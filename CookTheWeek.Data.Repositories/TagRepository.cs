namespace CookTheWeek.Data.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data.Models;

    public class TagRepository : ITagRepository
    {
        private readonly CookTheWeekDbContext dbContext;

        public TagRepository(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <inheritdoc/>
        public IQueryable<Tag> GetAllQuery()
        {
            return dbContext
                .Tags
                .AsNoTracking()
                .AsQueryable();
        }

        /// <inheritdoc/>
        public async Task<Tag?> GetByIdAsync(int tagId)
        {
            return await dbContext
                .Tags
                .Where(t => t.Id == tagId)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task AddAsync(Tag tag)
        {
            await dbContext.Tags.AddAsync(tag);
            await dbContext.SaveChangesAsync(); 
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Tag tag)
        {
            dbContext.Update(tag);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(Tag tag)
        {
            dbContext.Tags.Remove(tag);
            await dbContext.SaveChangesAsync();
        }
    }
}
