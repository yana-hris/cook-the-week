namespace CookTheWeek.Common.Exceptions
{

    public class RecordNotFoundException : CustomExceptionBase
    {
        public RecordNotFoundException(string message, Exception? ex)
        : base(message)
        {
        }

        public override string ErrorCode => "RECORD_NOT_FOUND";
    }
}
