namespace CookTheWeek.Common.Exceptions
{
    public abstract class CustomExceptionBase : Exception
    {
        protected CustomExceptionBase(string message) 
            : base(message) 
        { 

        }

        // Abstract method that must be implemented by derived classes
        public abstract string ErrorCode { get; }

        // Concrete method with implementation
        public string GetFormattedMessage()
        {
            return $"Error: {Message}";
        }
    }
}
