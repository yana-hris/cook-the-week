namespace CookTheWeek.Common
{
    public static class NotificationMessagesConstants
    {
        public const string ErrorMessage = "ErrorMessage";
        public const string WarningMessage = "WarnMessage";
        public const string InformationMessage = "InfoMessage";
        public const string SuccessMessage = "SuccessMessage";

        

        public const string SuccessfulEmailSentMessage = "Thank you for your message. We will get back to you soon.";
        public const string UnsuccessfulEmailSentMessage = "Error sending email. Please try again later.";

        // Modal text for missing recipes
        public const string MissingRecipesModalHeading = "Meal Plan Copied with Missing Recipes";
        public const string MissingRecipesModalContent = "Some recipes in your meal plan could not be found and will be removed.";
        public const string MissingRecipesModalIconClass = "fa-solid fa-triangle-exclamation warning";

        // Modal text for delete confirmation
        public const string DeleteModalHeading = "Confirm Delete";
        public const string DeleteModalIconClass = "fa-solid fa-triangle-exclamation error";
        
    }

    public static class TempDataConstants
    {
        public const string IsAdmin = "IsAdmin";
        public const string MealPlanStoredModel = "MealPlanAddFormModel";
        public const string SubmissionSuccess = "SubmissionSuccess";
        public const string MissingRecipesMessage = "MissingRecipesMessage";
    }
}
