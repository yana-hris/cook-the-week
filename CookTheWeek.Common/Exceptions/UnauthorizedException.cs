namespace CookTheWeek.Common.Exceptions
{
    public class UnauthorizedException : CustomExceptionBase
    {
        public UnauthorizedException(string message)
        : base(message)
        {
        }

        public override string ErrorCode => "UNAUTHORIZED";
    }
}
