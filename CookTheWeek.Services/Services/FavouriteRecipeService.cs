namespace CookTheWeek.Services.Data.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Models.FavouriteRecipe;
    using CookTheWeek.Services.Data.Services.Interfaces;

    using static CookTheWeek.Common.ExceptionMessagesConstants;

    public class FavouriteRecipeService : IFavouriteRecipeService
    {
        private readonly IFavouriteRecipeRepository favouriteRecipeRepository;
        private readonly IRecipeValidationService recipeValidator;
        private readonly ILogger<FavouriteRecipeService> logger;
        private readonly Guid userId;

        public FavouriteRecipeService(IFavouriteRecipeRepository favouriteRecipeRepository,
            IUserContext userContext,
            ILogger<FavouriteRecipeService> logger,
            IRecipeValidationService recipeValidator)
        {
            this.favouriteRecipeRepository = favouriteRecipeRepository;
            this.recipeValidator = recipeValidator;
            this.logger = logger;
            this.userId = userContext.UserId;   
        }


        /// <inheritdoc/>
        public async Task TryToggleLikesAsync(FavouriteRecipeServiceModel model)
        {
            Guid serviceUserId = model.UserId;
            Guid recipeId = model.RecipeId;

            await recipeValidator.ValidateRecipeExistsAsync(recipeId);

            // Validate user authorization
            if (userId != default &&
                serviceUserId != default &&
                userId != serviceUserId)
            {
                logger.LogError($"Unauthorized access attempt: User {userId} attempted to like/unlike recipe {recipeId} without necessary permissions.");
                throw new UnauthorizedUserException(UnauthorizedExceptionMessages.UserNotLoggedInExceptionMessage);
            }

            var existingLike = await GetRecipeLikeIfExistsAsync(model.RecipeId);

            if (existingLike != null)
            {
                await DeleteLikeAsync(existingLike);
            }
            else
            {
                await AddLikeAsync(model.RecipeId);
            }
        }

        /// <inheritdoc/>
        public async Task<ICollection<string>> GetAllRecipeIdsLikedByCurrentUserAsync()
        {
            ICollection<string> allLikedIds = await favouriteRecipeRepository
                .GetAllQuery()
                .Where(fr => fr.UserId == userId)
                .Select(fr => fr.RecipeId.ToString())
                .ToListAsync();

            return allLikedIds;
        }

        /// <inheritdoc/>
        public async Task<int?> GetRecipeTotalLikesAsync(Guid recipeId)
        {
            int? totalLikes = await favouriteRecipeRepository
                .GetAllQuery()
                .Where(fr => fr.RecipeId == recipeId)
                .CountAsync();

            return totalLikes;
        }

        /// <inheritdoc/>
        public async Task<FavouriteRecipe?> GetRecipeLikeIfExistsAsync(Guid recipeId)
        {
            return await favouriteRecipeRepository.GetByIdAsync(userId, recipeId);
        }

        /// <inheritdoc/>
        public async Task SoftDeleteAllByRecipeIdAsync(Guid recipeId)
        {
            ICollection<FavouriteRecipe> allLikes = await GetAllByRecipeIdAsync(recipeId);

            if (allLikes.Any())
            {
                foreach (var like in allLikes)
                {
                    like.IsDeleted = true;
                }

                await favouriteRecipeRepository.UpdateRangeAsync(allLikes);
            }
        }

        



        // PRIVATE METHODS:

        /// <summary>
        /// Helper method that gets a collection of all user likes for a given recipe by its ID
        /// </summary>
        /// <param name="recipeId">The recipe ID</param>
        /// <returns>A collection of FavouriteRecipes</returns>
        private async Task<ICollection<FavouriteRecipe>> GetAllByRecipeIdAsync(Guid recipeId)
        {
            return await favouriteRecipeRepository
                .GetAllQuery()
                .Where(fr => fr.RecipeId == recipeId)
                .ToListAsync();
        }

        /// <summary>
        /// Deletes a user like for a specific recipe
        /// </summary>
        /// <param name="FavouriteRecipe"></param>
        /// <returns></returns>
        private async Task DeleteLikeAsync(FavouriteRecipe like)
        {
            await favouriteRecipeRepository.DeleteAsync(like);
        }

        /// <summary>
        /// Adds a like by a given user id for a recipe with a given recipe id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        private async Task AddLikeAsync(Guid recipeId)
        {

            FavouriteRecipe favouriteRecipe = new FavouriteRecipe()
            {
                UserId = userId,
                RecipeId = recipeId
            };

            await favouriteRecipeRepository.AddAsync(favouriteRecipe);
        }

    }
}
