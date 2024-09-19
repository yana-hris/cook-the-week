namespace CookTheWeek.Common.Exceptions
{
    public class UnauthorizedUserException : CustomExceptionBase
    {
        public UnauthorizedUserException(string message)
        : base(message)
        {
        }

        public override string ErrorCode => "UNAUTHORIZED";
    }
}
