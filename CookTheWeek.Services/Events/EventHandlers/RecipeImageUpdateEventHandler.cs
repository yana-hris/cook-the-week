namespace CookTheWeek.Services.Data.Events.EventHandlers
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Services.Interfaces;

    public class RecipeImageUpdateEventHandler:IDomainEventHandler<RecipeImageUpdateEvent>
    {
        private readonly IImageService imageService;
        private readonly IRecipeRepository recipeRepository;
        private readonly ILogger logger;

        public RecipeImageUpdateEventHandler(IImageService imageService,
            ILogger<RecipeImageUpdateEventHandler> logger,
            IRecipeRepository recipeRepository)
        {
            this.imageService = imageService;
            this.recipeRepository = recipeRepository;
            this.logger = logger;
        }

        public async Task HandleAsync(RecipeImageUpdateEvent @event)
        {
            try
            {
                var imageUrl = await imageService.UploadImageAsync(@event.ExternalImageUrl);
                

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    var recipe = await recipeRepository
                    .GetByIdQuery(@event.RecipeId)
                    .FirstOrDefaultAsync();

                    if (recipe != null)
                    {
                        recipe.InternalImageUrl = imageUrl;
                        await recipeRepository.SaveChangesAsync();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to upload image for recipe: {@event.RecipeId}, using external URL: {@event.ExternalImageUrl}");
                
            }
        }

    }
}
