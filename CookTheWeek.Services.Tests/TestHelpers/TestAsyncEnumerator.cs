namespace CookTheWeek.Services.Tests.TestHelpers
{
   
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an asynchronous enumerator for testing purposes. This class allows 
    /// for simulating asynchronous enumeration over a collection in unit tests.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestAsyncEnumerator{T}"/> class.
        /// </summary>
        /// <param name="inner">The inner enumerator to wrap.</param>
        public TestAsyncEnumerator(IEnumerator<T> inner)
        {
            this.inner = inner;
        }

        /// <summary>
        /// Disposes the enumerator asynchronously.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous dispose operation.</returns>
        public ValueTask DisposeAsync()
        {
            inner.Dispose();
            return ValueTask.CompletedTask;
        }

        /// <summary>
        /// Moves the enumerator asynchronously to the next element in the collection.
        /// </summary>
        /// <returns>A <see cref="ValueTask{TResult}"/> that represents the asynchronous operation, 
        /// containing a boolean that is true if the enumerator was successfully advanced to the next element; false otherwise.</returns>

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(inner.MoveNext());
        }

        /// <summary>
        /// Gets the current element in the collection.
        /// </summary>
        public T Current => inner.Current;
    }
}
