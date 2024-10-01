namespace CookTheWeek.Services.Data.Services
{
    using CookTheWeek.Common.HelperMethods;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Models.FavouriteRecipe;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class FavouriteRecipeService : IFavouriteRecipeService
    {
        private readonly IFavouriteRecipeRepository favouriteRecipeRepository;
        private readonly IValidationService validationService;

        public FavouriteRecipeService(IFavouriteRecipeRepository favouriteRecipeRepository,
            IValidationService validationService)
        {
            this.favouriteRecipeRepository = favouriteRecipeRepository;
            this.validationService = validationService;    
        }


        /// <inheritdoc/>
        public async Task TryToggleLikes(FavouriteRecipeServiceModel model)
        {
            var result = await validationService.ValidateLikeOrUnlikeRecipeAsync(model);

            string userId = model.UserId;
            string recipeId = model.RecipeId;

            bool isAlreadyAdded = await HasUserByIdLikedRecipeById(userId, recipeId);

            if (isAlreadyAdded)
            {
                await DeleteLikeAsync(userId, recipeId);
            }
            else
            {
                await AddLikeAsync(userId, recipeId);
            }
        }

        /// <inheritdoc/>
        public async Task<ICollection<FavouriteRecipe>> GetAllRecipesLikedByUserIdAsync(string userId)
        {
            ICollection<FavouriteRecipe> likes = await favouriteRecipeRepository
                .GetAllQuery()
                .Where(fr => GuidHelper.CompareGuidStringWithGuid(userId, fr.UserId))
                .ToListAsync();

            return likes;
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
        public async Task<bool> HasUserByIdLikedRecipeById(string userId, string recipeId)
        {
            var like = await favouriteRecipeRepository.GetByIdAsync(userId, recipeId);

            return like != null;    
        }

        /// <inheritdoc/>
        public async Task DeleteAllRecipeLikesAsync(string recipeId)
        {
            ICollection<FavouriteRecipe> likesByRecipeId = await favouriteRecipeRepository
                .GetAllQuery()
                .Where(fr => GuidHelper.CompareGuidStringWithGuid(recipeId, fr.RecipeId))
                .ToListAsync();

            await favouriteRecipeRepository.DeleteAllAsync(likesByRecipeId);
        }



        // PRIVATE METHODS:

        /// <summary>
        /// Deletes a user like for a specific recipe if it exists. Otherwise does nothing
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        private async Task DeleteLikeAsync(string userId, string recipeId)
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
        private async Task AddLikeAsync(string userId, string recipeId)
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
