namespace CookTheWeek.Services.Data.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Common.HelperMethods;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Models.FavouriteRecipe;
    using CookTheWeek.Services.Data.Services.Interfaces;

    public class FavouriteRecipeService : IFavouriteRecipeService
    {
        private readonly IFavouriteRecipeRepository favouriteRecipeRepository;
        private readonly IValidationService validationService;
        private readonly string? userId;

        public FavouriteRecipeService(IFavouriteRecipeRepository favouriteRecipeRepository,
            IUserContext userContext,
            IValidationService validationService)
        {
            this.favouriteRecipeRepository = favouriteRecipeRepository;
            this.validationService = validationService;    
            this.userId = userContext.UserId;   
        }


        /// <inheritdoc/>
        public async Task TryToggleLikes(FavouriteRecipeServiceModel model)
        {
            await validationService.ValidateUserLikeForRecipe(model);
            string recipeId = model.RecipeId;

            bool isAlreadyAdded = await HasUserByIdLikedRecipeById(recipeId);

            if (isAlreadyAdded)
            {
                await DeleteLikeAsync(recipeId);
            }
            else
            {
                await AddLikeAsync(recipeId);
            }
        }

        /// <inheritdoc/>
        public async Task<ICollection<string>> GetAllRecipeIdsLikedByCurrentUserAsync()
        {
            ICollection<string> allLikedIds = await favouriteRecipeRepository
                .GetAllQuery()
                .Where(fr => GuidHelper.CompareGuidStringWithGuid(userId, fr.UserId))
                .Select(fr => fr.RecipeId.ToString())
                .ToListAsync();

            return allLikedIds;
        }

        /// <inheritdoc/>
        public async Task<int?> GetRecipeTotalLikesAsync(string recipeId)
        {
            int? totalLikes = await favouriteRecipeRepository
                .GetAllQuery()
                .Where(fr => GuidHelper.CompareGuidStringWithGuid(recipeId, fr.RecipeId))
                .CountAsync();

            return totalLikes;
        }

        /// <inheritdoc/>
        public async Task<bool> HasUserByIdLikedRecipeById(string recipeId)
        {
            var like = await favouriteRecipeRepository.GetByIdAsync(userId, recipeId);

            return like != null;    
        }

        /// <inheritdoc/>
        public async Task SoftDeleteAllByRecipeIdAsync(string recipeId)
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

        /// <inheritdoc/>
        public async Task HardDeleteAllByRecipeIdAsync(string recipeId)
        {
            ICollection<FavouriteRecipe> likesByRecipeId = await GetAllByRecipeIdAsync(recipeId);

            if (likesByRecipeId.Any())
            {
                await favouriteRecipeRepository.DeleteRangeAsync(likesByRecipeId);
            }
        }

        





        // PRIVATE METHODS:

        /// <summary>
        /// Helper method that gets a collection of all user likes for a given recipe by its ID
        /// </summary>
        /// <param name="recipeId">The recipe ID</param>
        /// <returns>A collection of FavouriteRecipes</returns>
        private async Task<ICollection<FavouriteRecipe>> GetAllByRecipeIdAsync(string recipeId)
        {
            return await favouriteRecipeRepository
                .GetAllQuery()
                .Where(fr => GuidHelper.CompareGuidStringWithGuid(recipeId, fr.RecipeId))
                .ToListAsync();
        }

        /// <summary>
        /// Deletes a user like for a specific recipe if it exists. Otherwise does nothing
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        private async Task DeleteLikeAsync(string recipeId)
        {
            
            FavouriteRecipe? like = await favouriteRecipeRepository.GetByIdAsync(userId, recipeId);

            if (like != null)
            {
                await favouriteRecipeRepository.DeleteAsync(like);
            }
        }

        /// <summary>
        /// Adds a like by a given user id for a recipe with a given recipe id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        private async Task AddLikeAsync(string recipeId)
        {

            FavouriteRecipe favouriteRecipe = new FavouriteRecipe()
            {
                UserId = Guid.Parse(userId),
                RecipeId = Guid.Parse(recipeId)
            };

            await favouriteRecipeRepository.AddAsync(favouriteRecipe);
        }

    }
}
