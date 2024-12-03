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
        public const string MissingRecipesModalHeading = "Missing Recipes";
        public const string MissingRecipesModalContent = "Some recipes in your meal plan could not be found and will be removed.";
        public const string MissingRecipesModalIconClass = "fa-solid fa-triangle-exclamation warning";

        // Modal text for delete confirmation
        public const string DeleteModalHeading = "Confirm Delete";
        public const string DeleteModalIconClass = "fa-solid fa-triangle-exclamation error";

        // Modal text for delete account confirmation
        public const string DeleteAccountModalHeading = "Delete Account Confirmation";
        public const string DeleteAccountModalMessage = "Are you sure you want to delete your account? This action is permanent and cannot be undone. All your saved recipes, meal plans, and account data will be permanently removed.";

        // Welcome Modal text for user
        public const string WelcomeGuestModalHeading = "Join our community today!";
        public const string WelcomeReturningUserModalHeading = "Welcome Back";
        public const string ActiveMealPlanModalContent = "Your active meal plan";
    }

    public static class TempDataConstants
    {
        public const string IsAdmin = "IsAdmin";
        public const string JustLoggedIn = "JustLoggedIn";
        public const string MealPlanStoredModel = "MealPlanAddFormModel";
        public const string SubmissionSuccess = "SubmissionSuccess";
        public const string MissingRecipesMessage = "MissingRecipesMessage";
    }
}
