﻿namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using CookTheWeek.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Category;

    using static Common.NotificationMessagesConstants;

    public class CategoryController : BaseAdminController
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        // Recipe Category Service
        [HttpGet]
        public async Task<IActionResult> AllRecipeCategories()
        {
            ICollection<RecipeCategorySelectViewModel> all = await categoryService
                .AllRecipeCategoriesAsync();

            return View(all);
        }

        [HttpGet]
        public IActionResult AddRecipeCategory()
        {
            RecipeCategoryAddFormModel model = new RecipeCategoryAddFormModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRecipeCategory(RecipeCategoryAddFormModel model)
        {
            bool categoryExists = await categoryService.RecipeCategoryExistsByNameAsync(model.Name);
            
            if(categoryExists)
            {
                ModelState.AddModelError(nameof(model.Name), $"Category with name {model.Name} already exists!");
            }

            if(!ModelState.IsValid)
            {
                ICollection<string> modelErrors = ModelState.Values.SelectMany(v => v.Errors)
                                   .Select(e => e.ErrorMessage)
                                   .ToList();
                var formattedErrors = string.Join(Environment.NewLine, modelErrors);
                TempData[ErrorMessage] = formattedErrors;
                return View(model);
            }

            try
            {
                await this.categoryService.AddRecipeCategoryAsync(model);
                TempData[SuccessMessage] = $"Recipe Category with name \"{model.Name}\" was successfully added!";
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return RedirectToAction("AllRecipeCategories");
        }

        [HttpGet]
        public async Task<IActionResult> EditRecipeCategory(int id)
        {
            bool exists = await categoryService.RecipeCategoryExistsByIdAsync(id);

            if(!exists)
            {
                return NotFound();
            }

            RecipeCategoryEditFormModel model = await this.categoryService.GetRecipeCategoryForEditByIdAsync(id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRecipeCategory(RecipeCategoryEditFormModel model)
        {
            bool existsById = await this.categoryService.RecipeCategoryExistsByIdAsync(model.Id);
            if(!existsById)
            {
                return NotFound(model);
            }

            bool existsByName = await this.categoryService.RecipeCategoryExistsByNameAsync(model.Name);
            if(existsByName)
            {
                int existingCategoryId = await this.categoryService.GetRecipeCategoryIdByNameAsync(model.Name);

                if(existingCategoryId != model.Id)
                {
                    ModelState.AddModelError(nameof(model.Name), $"Recipe Category with name \"{model.Name}\" already exists!");
                }
            }           

            if (ModelState.IsValid)
            {
                try
                {
                    await categoryService.EditRecipeCategoryAsync(model);
                    TempData[SuccessMessage] = $"Recipe Category \"{model.Name}\" edited successfully!";
                    return RedirectToAction("AllRecipeCategories");
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }

            return View(model);
        }
        public async Task<IActionResult> DeleteRecipeCategory(int id)
        {
            bool exists = await this.categoryService.RecipeCategoryExistsByIdAsync(id);

            if(!exists)
            {
                return NotFound();
            }
            
            try
            {
                await this.categoryService.DeleteRecipeCategoryById(id);
                TempData[SuccessMessage] = $"Recipe Category successfully deleted!";
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return RedirectToAction("AllRecipeCategories");
        }

        // Ingredient Category Service
        public async Task<IActionResult> AllIngredientCategories()
        {
            ICollection<IngredientCategorySelectViewModel> all = await categoryService
               .AllIngredientCategoriesAsync();

            return View(all);
        }

        [HttpGet]
        public IActionResult AddIngredientCategory()
        {
            IngredientCategoryAddFormModel model = new IngredientCategoryAddFormModel();
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddIngredientCategory(IngredientCategoryAddFormModel model)
        {
            bool categoryExists = await this.categoryService.IngredientCategoryExistsByNameAsync(model.Name);

            if(categoryExists)
            {
                ModelState.AddModelError(nameof(model.Name), $"Category with name {model.Name} already exists!");
            }

            if (!ModelState.IsValid)
            {
                ICollection<string> modelErrors = ModelState.Values.SelectMany(v => v.Errors)
                                   .Select(e => e.ErrorMessage)
                                   .ToList();
                var formattedErrors = string.Join(Environment.NewLine, modelErrors);
                TempData[ErrorMessage] = formattedErrors;
                return View(model);
            }

            try
            {
                await this.categoryService.AddIngredientCategoryAsync(model);
                TempData[SuccessMessage] = $"Ingredient Category with name \"{model.Name}\" was successfully added!";
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return RedirectToAction("AllIngredientCategories");
        }

        [HttpGet]
        public async Task<IActionResult> EditIngredientCategory(int id)
        {
            bool exists = await categoryService.IngredientCategoryExistsByIdAsync(id);

            if (!exists)
            {
                return NotFound();
            }

            IngredientCategoryEditFormModel model = await this.categoryService.GetIngredientCategoryForEditByIdAsync(id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditIngredientCategory(IngredientCategoryEditFormModel model)
        {
            bool existsById = await this.categoryService.IngredientCategoryExistsByIdAsync(model.Id);
            if (!existsById)
            {
                return NotFound(model);
            }

            bool existsByName = await this.categoryService.IngredientCategoryExistsByNameAsync(model.Name);
            if (existsByName)
            {
                int existingCategoryId = await this.categoryService.GetIngredientCategoryIdByNameAsync(model.Name);

                if (existingCategoryId != model.Id)
                {
                    ModelState.AddModelError(nameof(model.Name), $"Ingredient Category with name \"{model.Name}\" already exists!");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await categoryService.EditIngredientCategoryAsync(model);
                    TempData[SuccessMessage] = $"Ingredient Category \"{model.Name}\" edited successfully!";
                    return RedirectToAction("AllIngredientCategories");
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteIngredientCategory(int id)
        {
            bool exists = await this.categoryService.IngredientCategoryExistsByIdAsync(id);

            if (!exists)
            {
                return NotFound();
            }

            try
            {
                await this.categoryService.DeleteIngredientCategoryById(id);
                TempData[SuccessMessage] = $"Ingredient Category successfully deleted!";
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return RedirectToAction("AllIngredientCategories");
        }
    }
}