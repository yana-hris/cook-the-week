namespace CookTheWeek.Services.Data
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using Interfaces;
    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    using Models.Ingredient;
    using Models.RecipeIngredient;
    using Web.ViewModels.Admin.IngredientAdmin;
    using Web.ViewModels.Admin.IngredientAdmin.Enums;

    public class IngredientService : IIngredientService
    {
        private readonly CookTheWeekDbContext dbContext;

        public IngredientService(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
       
        public async Task<AllIngredientsFilteredAndPagedServiceModel> AllAsync(AllIngredientsQueryModel queryModel)
        {
            IQueryable<Ingredient> ingredientsQuery = this.dbContext
                .Ingredients
                .Include(i => i.Category)
                .AsNoTracking()
                .AsQueryable();

            if(!string.IsNullOrWhiteSpace(queryModel.Category))
            {
                ingredientsQuery = ingredientsQuery
                    .Where(i => i.Category.Name == queryModel.Category);
            }

            if(!string.IsNullOrWhiteSpace(queryModel.SearchString))
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

            ICollection<IngredientAllViewModel> allIngredients = await ingredientsQuery
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
                Ingredients = allIngredients
            };
        }
        public async Task<int> AddAsync(IngredientAddFormModel model)
        {
            Ingredient ingredient = new Ingredient()
            {
                Name = model.Name,
                CategoryId = model.CategoryId
            };

            await this.dbContext.Ingredients.AddAsync(ingredient);
            await this.dbContext.SaveChangesAsync();

            return ingredient.Id;
        }
        public async Task EditAsync(IngredientEditFormModel model)
        {
            Ingredient? ingredient = await this.dbContext
                .Ingredients
                .Where(i => i.Id == model.Id)
                .FirstOrDefaultAsync();

            if(ingredient != null)
            {
                ingredient.Name = model.Name;
                ingredient.CategoryId = model.CategoryId;
            }

            await this.dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<RecipeIngredientSuggestionServiceModel>> GenerateIngredientSuggestionsAsync(string input)
        {
            string wildCard = $"%{input.ToLower()}%";

            return  await this.dbContext
                .Ingredients
                .AsNoTracking()
                .Where(i => EF.Functions.Like(i.Name.ToLower(), wildCard))
                .Select(i => new RecipeIngredientSuggestionServiceModel()
                {
                    Id = i.Id,
                    Name = i.Name,
                }).ToListAsync();
        }
        public async Task<IngredientEditFormModel> GetForEditByIdAsync(int id)
        {
            IngredientEditFormModel? model = await this.dbContext.Ingredients
                .Where(i => i.Id == id)
                .AsNoTracking()
                .Select(i => new IngredientEditFormModel()
                {
                    Id = i.Id,
                    Name = i.Name,
                    CategoryId = i.CategoryId
                })
                .FirstOrDefaultAsync();

            return model;
        }
        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await this.dbContext
                .Ingredients
                .AsNoTracking()
                .AnyAsync(i => i.Id == id);
        }
        public async Task<bool> ExistsByNameAsync(string name)
        {
            bool exists = await this.dbContext
                .Ingredients
                .AsNoTracking()
                .AnyAsync(i => i.Name.ToLower() == name.ToLower());

            return exists;
        }

        public async Task<int?> GetIdByNameAsync(string name)
        {
            return await this.dbContext
                .Ingredients
                .Where(i => i.Name.ToLower() == name.ToLower())
                .Select(i => i.Id)
                .FirstOrDefaultAsync();
        }
        public async Task<int> AllCountAsync()
        {
            return await this.dbContext
                .Ingredients
                .CountAsync();
        }
        public async Task DeleteById(int id)
        {
            Ingredient ingredient = await this.dbContext
                .Ingredients
                .FirstAsync(i => i.Id == id);

            this.dbContext.Ingredients.Remove(ingredient);
            await this.dbContext.SaveChangesAsync();
        }

    }
}
