namespace CookTheWeek.Services.Data.Services
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class RatingService : IRatingService
    {
        private readonly IRatingRepository ratingRepository;
        private readonly ILogger<RatingService> logger;

        public RatingService(IRatingRepository ratingRepository, 
            ILogger<RatingService> logger)
        {
            this.ratingRepository = ratingRepository;
            this.logger = logger;
        }
        public async Task SoftDeleteAllByRecipeIdAsync(Guid recipeId)
        {
            ICollection<RecipeRating> allRatings = await GetAllByRecipeIdAsync(recipeId);

            if (allRatings.Any())
            {
                foreach (var rating in allRatings)
                {
                    rating.IsDeleted = true;
                }

                await ratingRepository.UpdateRangeAsync(allRatings);
            }
        }


        // PRIVATE METHODS:

        /// <summary>
        /// Helper method that gets a collection of all user ratings for a given recipe by its ID
        /// </summary>
        /// <param name="recipeId">The recipe ID</param>
        /// <returns>A collection of RecipeRatings</returns>
        private async Task<ICollection<RecipeRating>> GetAllByRecipeIdAsync(Guid recipeId)
        {
            return await ratingRepository
                .GetAllQuery()
                .Where(r => r.RecipeId == recipeId) 
                .ToListAsync();
        }
    }
}
