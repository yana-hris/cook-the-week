namespace CookTheWeek.Common
{
    public static class EntityValidationConstants
    {
        public static class Recipe
        {
            public const string RecipeNotFoundErrorMessage = "Recipe not found";
            public const string RecipeSuccessfullySavedMessage = "Your recipe was successfully added!";
            public const string RecipeOwnerErrorMessage = "You must be the owner of this recipe to edit or delete it";

            // Id
            public const string InvalidRecipeIdErrorMessage = "Invalid recipeId";
                        
            // Title
            public const int TitleMinLength = 3;
            public const int TitleMaxLength = 100;

            public const string TitleRequiredErrorMessage = "Title is required";
            public const string TitleMinLengthErrorMessage = "The Title must be at least 3 characters long";
            public const string TitleMaxLengthErrorMessage = "The Title length cannot exceed 100 characters";

            // Description
            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 1500;

            public const string DescriptionRangeErrorMessage = "The Description must be between 10 and 1500 characters long";

            // Image URL
            public const int ImageUlrMinLength = 15;
            public const int ImageUlrMaxLength = 2048;
            public const string UrlPattern = @"^(http(s)?://)?([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}(:[0-9]{1,5})?(/.*)?$";

            public const string ImageRequiredErrorMessage = "Your image URL is required";
            public const string ImageInvalidErrorMessage = "Invalid URL";
            public const string ImageRangeErrorMessage = "Invalid URL length";

            // Meal Type (RecipeCategoryId)
            public const string RecipeCategoryIdRequiredErrorMessage = "Meal type is required";
            public const string RecipeCategoryIdInvalidErrorMessage = "Invalid meal type";

            // Servings
            public const int ServingsMinValue = 1;
            public const int ServingsMaxValue = 20;

            public static int[] ServingsOptions =
            [
                1,
                2,
                4,
                6,
                8,
                10,
                12
            ];

            public const string ServingsRequiredErrorMessage = "Servings number is required";
            public const string ServingsRangeErrorMessage = "Servings can be between 1 and 20";

            // Minutes / Cooking Time
            public const int CookingTimeMinValue = 10;
            public const int CookingTimeMaxValue = 720;

            public const string CookingTimeRequiredErrorMessage = "Cooking time (in minutes) is required";
            public const string CookingTimeRangeErrorMessage = "Cooking time minutes must be between 10 and 720";

            // Steps Collection
            public const string StepsRequiredErrorMessage = "At leas one step is required";

            // Ingredients Collection
            public const string IngredientsRequiredErrorMessage = "At least one ingredient is required";
            
        }

        public static class Step
        {
            public const int StepDescriptionMinLength = 5;
            public const int StepDescriptionMaxLength = 1000;

            public const string StepRequiredErrorMessage = "Step Description is required";
            public const string StepDescriptionRangeErrorMessage = "The cooking step description can be between 5 and 1000 characters long";
        }
        
        public static class RecipeIngredient
        {
            // Name
            public const int RecipeIngredientNameMinLength = 2;
            public const int RecipeIngredientNameMaxLength = 50;

            public const string RecipeIngredientNameRequiredErrorMessage = "Ingredient name is required";
            public const string RecipeIngredientNameRangeErrorMessage = "Ingredient name cannot be shorter than 2 and longer than 50 characters";
            public const string RecipeIngredientInvalidErrorMessage = "Invalid ingredient";


            // Qty
            public const int QtyWholeMinValue = 1;
            public const int QtyWholeMaxValue = 9999;

            public const double QtyMinDecimalValue = 0.01;
            public const double QtyMaxDecimalValue = 9999.99;

            //public const string QtyRegularExpression = @"^\d+(\.\d{1,2})?$";
            
            public const string QtyRequiredErrorMessage = "Qty is required";

            // Measure            
            public const string MeasureRequiredErrorMessage = "Unit/measure is required";
            public const string MeasureRangeErrorMessage = "Invalid unit/measure";

            // Specification
            public const string SpecificationRangeErrorMessage = "Invalid specification";

        }

        public static class RecipeIngredientQty
        {
            
            public const string MissingFormInputErrorMessage = "Required either decimal or fractional quantity";

            public const string MissingFractionalOrWholeInputMessage = "Required either a whole and/or fractional quantity";

            public const string InvalidDecimalRangeErrorMessage = "Qty must be in the range [0.001-9999.99]";

            public const string InvalidFractionErrorMessage = "Invalid fraction selection";

            public const string InvalidWholeQtyErrorMessage = "Qty must be in the range [1-9999]";
        }

        public static class Measure
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 50;
        }

        public static class Specification
        {
            public const int DescriptionMinLength = 2;
            public const int DescriptionMaxLength = 50;
        }

        // All categories TCategory (RecipeCategory, IngredientCategory)
        public static class Category
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 50;

            public const string CategoryExistsErrorMessage = "Category with this name already exists.";
        }
        
        public static class ApplicationUser
        {            
            public const int UsernameMinLength = 3;
            public const int UsernameMaxLength = 50;

            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 100;

            public const string LoginFailedErrorMessage = "Login failed: Invalid username or password.";
            public const string AccountLockedErrorMessage = "Your account is locked out. Kindly wait for 5 minutes and try again";

            public const string UsernameAlreadyExistsErrorMessage = "User with such username already exists";
            public const string EmailAlreadyExistsErrorMessage = "User with such email already exists";
            public const string AlreadyHaveAccountErrorMessage = "Are you sure you don`t already have an account? Go to <a href=\"/User/Login\">Login</a>";
            public const string UserNotFoundErrorMessage = "User with such id does not exist!";
            public const string InvalidUserIdErrorMessage = "The current user has another user id!";
        }

        public static class Meal
        {
            public const int MinServingSize = 1;
            public const int MaxServingSize = 12;

            public const string MealServingsRequiredErrorMessage = "Please enter serving size";
            public const string MealServingsRangeErrorMessage = "Please enter a valid serving size [1 - 12]";

            public const string DateRequiredErrorMessage = "Please select date";
            public const string DateRangeErrorMessage = "Please choose a valid cooking date";
        }

        public static class MealPlan
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 50;

            public const string MealPlanNotFoundErrorMessage = "Meal plan not found";
            public const string MealPlanOwnerErrorMessage = "You must be the owner of this meal plan to edit or delete it";

            public const string MealPlanSuccessfulDeleteMessage = "Meal Plan deleted!";
            public const string MealPlanSuccessfulSaveMessage = "Meal plan saved!";

            public const string NameRequiredErrorMessage = "Meal plan name is required";
            public const string NameLengthErrorMessage = "Meal plan name cannot be shorter than 2, and longer than 50 characters";

            public const string MealsRequiredErrorMessage = "At lease one meal is required";
            
        }

        public static class ErrorMessages
        {
            public const string InvalidInputErrorMessage = "Invalid input. Please provide valid content.";
            

            public static readonly Dictionary<string, string> RecipeValidationErrorMessages =
                new Dictionary<string, string>()
                {
                    { "TitleRequiredErrorMessage", Recipe.TitleRequiredErrorMessage },
                    { "TitleMinLengthErrorMessage", Recipe.TitleMinLengthErrorMessage },
                    { "TitleMaxLengthErrorMessage", Recipe.TitleMaxLengthErrorMessage },
                    { "DescriptionRangeErrorMessage", Recipe.DescriptionRangeErrorMessage },
                    { "ServingsRequiredErrorMessage", Recipe.ServingsRequiredErrorMessage },
                    { "ServingsRangeErrorMessage", Recipe.ServingsRangeErrorMessage },
                    { "ImageRequiredErrorMessage", Recipe.ImageRequiredErrorMessage },
                    { "ImageInvalidErrorMessage", Recipe.ImageInvalidErrorMessage },
                    { "ImageRangeErrorMessage", Recipe.ImageRangeErrorMessage },
                    { "CookingTimeRequiredErrorMessage", Recipe.CookingTimeRequiredErrorMessage },
                    { "CookingTimeRangeErrorMessage", Recipe.CookingTimeRangeErrorMessage },
                    { "RecipeCategoryIdRequiredErrorMessage", Recipe.RecipeCategoryIdRequiredErrorMessage },
                    { "StepsRequiredErrorMessage", Recipe.StepsRequiredErrorMessage },
                    { "StepDescriptionRangeErrorMessage", Step.StepDescriptionRangeErrorMessage },
                    { "IngredientsRequiredErrorMessage", Recipe.IngredientsRequiredErrorMessage },
                    { "RecipeIngredientNameRequiredErrorMessage", RecipeIngredient.RecipeIngredientNameRequiredErrorMessage },
                    { "RecipeIngredientNameRangeErrorMessage", RecipeIngredient.RecipeIngredientNameRangeErrorMessage },
                    { "MeasureRequiredErrorMessage", RecipeIngredient.MeasureRequiredErrorMessage },
                    { "MissingFormInputErrorMessage", RecipeIngredientQty.MissingFormInputErrorMessage },
                    { "MissingFractionalOrWholeInputMessage", RecipeIngredientQty.MissingFractionalOrWholeInputMessage },
                    { "InvalidDecimalRangeErrorMessage", RecipeIngredientQty.InvalidDecimalRangeErrorMessage },
                    { "InvalidFractionErrorMessage", RecipeIngredientQty.InvalidFractionErrorMessage },
                    { "InvalidWholeQtyErrorMessage", RecipeIngredientQty.InvalidWholeQtyErrorMessage },
                    {"InvalidInputErrorMessage", InvalidInputErrorMessage },
                };

            public static readonly Dictionary<string, string> ToastrMessages =
                new Dictionary<string, string>()
                {
                    {"MealsRequiredErrorMessage", MealPlan.MealsRequiredErrorMessage},
                };
        };

       

        public static class ValidationConstants
        {
            public static readonly Dictionary<string, int> RecipeValidationConstants =
                new Dictionary<string, int>()
                {
                    {"TitleMinLength", Recipe.TitleMinLength },
                    {"TitleMaxLength", Recipe.TitleMaxLength },
                    {"DescriptionMinLength", Recipe.DescriptionMinLength },
                    {"DescriptionMaxLength", Recipe.DescriptionMaxLength },
                    {"ServingsMinValue", Recipe.ServingsMinValue },
                    {"ServingsMaxValue", Recipe.ServingsMaxValue },
                    {"ImageUlrMinLength", Recipe.ImageUlrMinLength },
                    {"ImageUlrMaxLength", Recipe.ImageUlrMaxLength },
                    {"CookingTimeMinValue", Recipe.CookingTimeMinValue },
                    {"CookingTimeMaxValue", Recipe.CookingTimeMaxValue },
                    {"StepDescriptionMinLength", Step.StepDescriptionMinLength },
                    {"StepDescriptionMaxLength", Step.StepDescriptionMaxLength },
                    {"RecipeIngredientNameMinLength", RecipeIngredient.RecipeIngredientNameMinLength },
                    {"RecipeIngredientNameMaxLength", RecipeIngredient.RecipeIngredientNameMaxLength },

                };
        }

        
    }
}
