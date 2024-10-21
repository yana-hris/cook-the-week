namespace CookTheWeek.Services.Tests.TestHelpers
{
    using System.Linq.Expressions;

    /// <summary>
    /// Represents an asynchronous enumerable for testing purposes. This class allows for 
    /// the simulation of <see cref="IQueryable"/> operations over an in-memory collection in asynchronous tests.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestAsyncEnumerable{T}"/> class with an enumerable.
        /// </summary>
        /// <param name="enumerable">The enumerable to wrap.</param>
        public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestAsyncEnumerable{T}"/> class with an expression.
        /// </summary>
        /// <param name="expression">The expression to use for the query.</param>
        public TestAsyncEnumerable(Expression expression) : base(expression)
        {
        }

        /// <summary>
        /// Gets the asynchronous enumerator for the collection.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>An asynchronous enumerator for the collection.</returns>
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        /// <summary>
        /// Gets an asynchronous enumerator that iterates through the collection.
        /// This method implements the <see cref="IAsyncEnumerable{T}.GetAsyncEnumerator"/> interface explicitly.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous iteration.</param>
        /// <returns>An asynchronous enumerator that can be used to iterate through the collection.</returns>
        IAsyncEnumerator<T> IAsyncEnumerable<T>.GetAsyncEnumerator(CancellationToken cancellationToken)
        {
            return GetAsyncEnumerator(cancellationToken);
        }

        /// <summary>
        /// Gets the query provider for this instance.
        /// </summary>
        IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
    }
}
