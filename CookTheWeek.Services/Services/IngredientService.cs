namespace CookTheWeek.Services.Data.Services
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Services.Data.Models.Ingredient;
    using CookTheWeek.Services.Data.Models.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.Admin.IngredientAdmin;
    using CookTheWeek.Web.ViewModels.Admin.IngredientAdmin.Enums;

    public class IngredientService : IIngredientService
    {
        private readonly IIngredientRepository ingredientRepository;

        public IngredientService(IIngredientRepository ingredientRepository)
        {
            this.ingredientRepository = ingredientRepository;
        }

        /// <inheritdoc/>
        public async Task<AllIngredientsFilteredAndPagedServiceModel> AllAsync(AllIngredientsQueryModel queryModel)
        {
            IQueryable<Ingredient> ingredientsQuery = 
                                ingredientRepository.GetAllQuery();

            if (!string.IsNullOrWhiteSpace(queryModel.Category))
            {
                ingredientsQuery = ingredientsQuery
                    .Where(i => i.Category.Name == queryModel.Category);
            }

            if (!string.IsNullOrWhiteSpace(queryModel.SearchString))
            {
                string wildCard = $"%{queryModel.SearchString.ToLower()}%";

                ingredientsQuery = ingredientsQuery
                    .Where(i => EF.Functions.Like(i.Name, wildCard));
            }

            ingredientsQuery = queryModel.IngredientSorting switch
            {
                IngredientSorting.NameDescending => ingredientsQuery
                    .OrderByDescending(i => i.Name),
                IngredientSorting.NameAscending => ingredientsQuery
                    .OrderBy(i => i.Name),
                IngredientSorting.IdDescending => ingredientsQuery
                    .OrderByDescending(i => i.Id),
                IngredientSorting.IdAscending => ingredientsQuery
                    .OrderBy(i => i.Id),
                _ => ingredientsQuery.OrderBy(i => i.Name)
            };

            ICollection<IngredientAllViewModel> resultIngredients = await ingredientsQuery
                .Skip((queryModel.CurrentPage - 1) * queryModel.IngredientsPerPage)
                .Take(queryModel.IngredientsPerPage)
                .Select(i => new IngredientAllViewModel()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Category = i.Category.Name,
                })
                .ToListAsync();

            int totalIngredients = ingredientsQuery.Count();

            return new AllIngredientsFilteredAndPagedServiceModel()
            {
                TotalIngredientsCount = totalIngredients,
                Ingredients = resultIngredients
            };
        }

        /// <inheritdoc/>
        public async Task<int> AddAsync(IngredientAddFormModel model)
        {
            Ingredient ingredient = new Ingredient()
            {
                Name = model.Name,
                CategoryId = model.CategoryId
            };

            return await ingredientRepository.AddAsync(ingredient);
        }

        /// <inheritdoc/>
        public async Task EditAsync(IngredientEditFormModel model)
        {
            Ingredient ingredient = await ingredientRepository.GetByIdAsync(model.Id);

            ingredient.Name = model.Name;
            ingredient.CategoryId = model.CategoryId;

            await ingredientRepository.UpdateAsync(ingredient);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<RecipeIngredientSuggestionServiceModel>> GenerateIngredientSuggestionsAsync(string input)
        {
            IQueryable<Ingredient> suggestions = ingredientRepository
                .GetAllBySearchStringQuery(input);

            var model = await suggestions.
                Select(s => new RecipeIngredientSuggestionServiceModel()
                {
                    Id = s.Id,
                    Name = s.Name,  
                }).ToListAsync();

            return model;
        }

        /// <inheritdoc/>
        public async Task<IngredientEditFormModel> GetForEditByIdAsync(int id)
        {
            Ingredient ingredient = await ingredientRepository.GetByIdAsync(id);

            IngredientEditFormModel model = new IngredientEditFormModel()
            { 
                Id = ingredient.Id,
                Name = ingredient.Name,
                CategoryId = ingredient.CategoryId
            };

            return model;
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await ingredientRepository.ExistsByIdAsync(id);
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await ingredientRepository.ExistsByNameAsync(name);
        }
        
        /// <inheritdoc/>
        public async Task<int?> AllCountAsync()
        {
            return await ingredientRepository.CountAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteByIdAsync(int id)
        {
            Ingredient ingredient = await ingredientRepository.GetByIdAsync(id);
            await ingredientRepository.DeleteAsync(ingredient);
        }

        /// <inheritdoc/>
        public async Task<Ingredient> GetByIdAsync(int ingredientId)
        {
            return await ingredientRepository.GetByIdAsync(ingredientId);
        }
    }
}
