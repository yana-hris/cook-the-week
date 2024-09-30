namespace CookTheWeek.Services.Data.Services
{
    using CookTheWeek.Common.HelperMethods;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class FavouriteRecipeService : IFavouriteRecipeService
    {
        private readonly IFavouriteRecipeRepository favouriteRecipeRepository;

        public FavouriteRecipeService(IFavouriteRecipeRepository favouriteRecipeRepository)
        {
            this.favouriteRecipeRepository = favouriteRecipeRepository;
        }

        /// <inheritdoc/>
        public async Task AddLikeAsync(string userId, string recipeId)
        {
            FavouriteRecipe favouriteRecipe = new FavouriteRecipe()
            {
                UserId = Guid.Parse(userId),
                RecipeId = Guid.Parse(recipeId)
            };

            await favouriteRecipeRepository.AddAsync(favouriteRecipe);
        }
        

        /// <inheritdoc/>
        public async Task DeleteLikeAsync(string userId, string recipeId)
        {
            FavouriteRecipe? like = await favouriteRecipeRepository.GetByIdAsync(userId, recipeId);

            if (like != null)
            {
                await favouriteRecipeRepository.DeleteAsync(like);
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

    }
}
