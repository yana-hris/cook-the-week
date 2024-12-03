namespace CookTheWeek.Services.Data.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class TagService : ITagService
    {
        private readonly ITagRepository tagRepository;

        private readonly ILogger<TagService> logger;

        public TagService(ITagRepository tagRepository,
                            ILogger<TagService> logger)
        {
            this.tagRepository = tagRepository;

            this.logger = logger;
        }
        public async Task<ICollection<SelectViewModel>> GetAllTagsAsync()
        {
            try
            {
                ICollection<SelectViewModel> all = await tagRepository
                .GetAllQuery()
                .Select(t => new SelectViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                })
                .ToListAsync();

                return all;
            }
            catch (Exception ex)
            {
                logger.LogError($"Tags Select View Model loading failed. Error message: {ex.Message}. Error Stacktrace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
