namespace CookTheWeek.Common.HelperMethods
{
    using Ganss.Xss;
    public class SanitizerHelper
    {
        private readonly HtmlSanitizer sanitizer;

        public SanitizerHelper()
        {
           this.sanitizer = new HtmlSanitizer();
        }
        public string SanitizeInput(string input)
        {
            return sanitizer.Sanitize(input);
        }
    }
}
