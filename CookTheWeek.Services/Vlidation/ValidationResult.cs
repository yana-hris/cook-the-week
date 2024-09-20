namespace CookTheWeek.Services.Data.Vlidation
{
    
    public class ValidationResult
    {
        public ValidationResult()
        {
            this.Errors = new Dictionary<string, string>();
            this.IsValid = true;
        }
        public bool IsValid { get; set; }

        public IDictionary<string, string> Errors { get; set; }
    }
}
