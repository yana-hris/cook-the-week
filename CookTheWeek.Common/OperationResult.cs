namespace CookTheWeek.Common
{
    
    /// <summary>
    /// A utility class returned as a result from Service mrthods, used by the controller 
    /// </summary>
    public class OperationResult
    {
        public bool Succeeded { get; set; }
        public Dictionary<string, string> Errors { get; private set; }
        public Dictionary<string, object> Data { get; private set; } // Support for additional data

        public OperationResult(bool succeeded, Dictionary<string, string> errors, Dictionary<string, object> data)
        {
            Succeeded = succeeded;
            Errors = errors ?? new Dictionary<string, string>();
            Data = data ?? new Dictionary<string, object>();
        }

        public static OperationResult Success(Dictionary<string, object> data = null)
        {
            return new OperationResult(true, null, data);
        }

        public static OperationResult Failure(Dictionary<string, string> errors)
        {
            return new OperationResult(false, errors, null);
        }
    }

    /// <summary>
    /// A generic utility class returned as a result from Service mrthods, used by the controller 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OperationResult<T>
    {
        /// <summary>
        /// Flag, indicating the result status
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// A collection of type Dictionary, containing any model errors, returned as a result of custom validation
        /// </summary>
        public IDictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// A parameter, if needed to be passed to the controller as a result of the service method (int, string, viewmodel, etc)
        /// </summary>
        public T? Value { get; set; }

        /// <summary>
        /// Factory method, used to return a successful result without the need to explicitly fill in all properties
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static OperationResult<T> Success(T value)
        {
            return new OperationResult<T> { Succeeded = true, Value = value };
        }

        /// <summary>
        /// Factory method, used to return a predefined failure result, accepting a dictionary with errors (kvp)
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static OperationResult<T> Failure(IDictionary<string, string> errors)
        {
            return new OperationResult<T> { Succeeded = false, Errors = errors };
        }
    }

   

}
