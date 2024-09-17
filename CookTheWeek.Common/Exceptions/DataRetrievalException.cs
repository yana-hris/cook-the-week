namespace CookTheWeek.Common.Exceptions
{

    public class DataRetrievalException : CustomExceptionBase
    {
        public DataRetrievalException(string message, Exception ex)
        : base(message)
        {
        }

        public override string ErrorCode => "DATA_RETRIEVAL_ERROR";
    }
}
