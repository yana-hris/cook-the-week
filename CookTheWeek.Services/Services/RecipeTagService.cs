namespace CookTheWeek.Services.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Data.Repositories;
    using Microsoft.EntityFrameworkCore;

    public class RecipeTagService : IRecipeTagService
    {
        private readonly IRecipeTagRepository recipeTagRepository;
        private readonly ILogger<RecipeTagService> logger;

        public RecipeTagService(IRecipeTagRepository recipeTagRepository,
            ILogger<RecipeTagService> logger)
        {
            this.recipeTagRepository = recipeTagRepository;
            this.logger = logger;
        }

        /// <inheritdoc/>  
        public ICollection<RecipeTag> CreateAll(List<int> selectedTagIds)
        {
            HashSet<RecipeTag> tagsToAdd = new HashSet<RecipeTag>();

            foreach (var tag in selectedTagIds)
            {
                RecipeTag newTag = new RecipeTag
                {
                    TagId = tag
                };

                tagsToAdd.Add(newTag);
            }

            return tagsToAdd;
        }

        /// <inheritdoc/>  
        public async Task<ICollection<RecipeTag>> UpdateAll(Guid id, List<int> selectedTagIds)
        {
            ICollection<RecipeTag> oldRecipeTags = await recipeTagRepository
                .GetAllTrackedQuery()
                .Where(rt => rt.RecipeId == id)
                .ToListAsync();

            HashSet<RecipeTag> updatedRecipeTags = new HashSet<RecipeTag>();

            if(selectedTagIds.Count > 0) 
            {
                foreach (var tagId in selectedTagIds)
                {
                    RecipeTag? existingTag = oldRecipeTags.FirstOrDefault(rt => rt.TagId == tagId);

                    if (existingTag != null)
                    {
                        updatedRecipeTags.Add(existingTag);
                    }
                    else
                    {
                        RecipeTag newTag = new RecipeTag
                        {
                            RecipeId = id,
                            TagId = tagId
                        };

                        updatedRecipeTags.Add(newTag);
                    }
                }
            }

            return updatedRecipeTags;
        }
    }
}
