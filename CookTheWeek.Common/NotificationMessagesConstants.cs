﻿namespace CookTheWeek.Common
{
    public static class NotificationMessagesConstants
    {
        public const string ErrorMessage = "ErrorMessage";
        public const string WarningMessage = "WarnMessage";
        public const string InformationMessage = "InfoMessage";
        public const string SuccessMessage = "SuccessMessage";

        

        public const string SuccessfulEmailSentMessage = "Thank you for your message. We will get back to you soon.";
        public const string UnsuccessfulEmailSentMessage = "Error sending email. Please try again later.";
    }

    public static class TempDataConstants
    {
        public const string IsAdmin = "IsAdmin";
        public const string MealPlanStoredModel = "MealPlanAddFormModel";
        public const string SubmissionSuccess = "SubmissionSuccess";
        public const string MissingRecipesMessage = "MissingRecipesMessage";
    }
}
