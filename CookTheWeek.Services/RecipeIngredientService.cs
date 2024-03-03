namespace CookTheWeek.Services
{
    
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using CookTheWeek.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;

    public class RecipeIngredientService : IRecipeIngredientService
    {
        private readonly CookTheWeekDbContext dbContext;

        public RecipeIngredientService(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<RecipeIngredientMeasureViewModel>> GetRecipeIngredientMeasuresAsync()
        {
            ICollection<RecipeIngredientMeasureViewModel> allMeasures = await this.dbContext
                .Measures
                .AsNoTracking()
                .Select(m => new RecipeIngredientMeasureViewModel()
                {
                    Id = m.Id,
                    Name = m.Name,

                }).ToListAsync();

            return allMeasures;
        }

        public async Task<ICollection<RecipeIngredientSpecificationViewModel>> GetRecipeIngredientSpecificationsAsync()
        {
            ICollection<RecipeIngredientSpecificationViewModel> allSpecs = await this.dbContext
                .Specifications
                .AsNoTracking()
                .Select(sp => new RecipeIngredientSpecificationViewModel()
                {
                    Id = sp.Id,
                    Description = sp.Description
                }).ToListAsync();

            return allSpecs;
        }

        //public async Task<int> AddAsync(RecipeIngredientFormViewModel model, string recipeId)
        //{
        //    RecipeIngredient recipeIngredient = new RecipeIngredient()
        //    {
        //        RecipeId = Guid.Parse(recipeId),
        //        Qty = model.Qty,
        //        MeasureId = model.MeasureId,
        //        SpecificationId = model.SpecificationId
        //    };

        //    await this.dbContext.RecipesIngredients
        //        .AddAsync(recipeIngredient);
        //    await this.dbContext.SaveChangesAsync();

        //    return recipeIngredient.IngredientId;
        //}

        public async Task<bool> IngredientIsAlreadyAddedAsync(string ingredientName, string recipeId)
        {
            return await this.dbContext.RecipesIngredients
                .AsNoTracking()
                .AnyAsync(ri => ri.RecipeId.ToString() == recipeId && ri.Ingredient.Name == ingredientName);
        }

        public async Task<bool> IngredientMeasureExistsAsync(int measureId)
        {
            return await this.dbContext.Measures
                .AsNoTracking()
                .AnyAsync(m => m.Id == measureId);
        }

        public async Task<bool> IngredientSpecificationExistsAsunc(int specificationId)
        {
            return await this.dbContext.Specifications
                .AsNoTracking()
                .AnyAsync(sp => sp.Id == specificationId);
        }
    }
}
