namespace CookTheWeek.Services.Tests.TestHelpers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;

    using Microsoft.EntityFrameworkCore.Query;


    /// <summary>
    /// Provides an asynchronous query provider for testing purposes. This class wraps an inner 
    /// <see cref="IQueryProvider"/> and allows asynchronous query execution using <see cref="IAsyncEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
    public class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestAsyncQueryProvider{TEntity}"/> class.
        /// </summary>
        /// <param name="inner">The inner <see cref="IQueryProvider"/> to wrap.</param>
        public TestAsyncQueryProvider(IQueryProvider inner)
        {
            this.inner = inner;
        }

        /// <summary>
        /// Creates a query for the specified expression.
        /// </summary>
        /// <param name="expression">The expression representing the query.</param>
        /// <returns>A new instance of an <see cref="IQueryable"/> for the specified expression.</returns>
        public IQueryable CreateQuery(Expression expression)
        {
            return new TestAsyncEnumerable<TEntity>(expression);
        }

        /// <summary>
        /// Creates a query of a specified type for the specified expression.
        /// </summary>
        /// <typeparam name="TElement">The type of the element in the query.</typeparam>
        /// <param name="expression">The expression representing the query.</param>
        /// <returns>A new instance of an <see cref="IQueryable{TElement}"/> for the specified expression.</returns>
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestAsyncEnumerable<TElement>(expression);
        }

        /// <summary>
        /// Executes the given expression as a query.
        /// </summary>
        /// <param name="expression">The expression representing the query.</param>
        /// <returns>The result of the query.</returns>
        public object Execute(Expression expression)
        {
            return inner.Execute(expression);
        }

        /// <summary>
        /// Executes the given expression as a query and returns a typed result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="expression">The expression representing the query.</param>
        /// <returns>The typed result of the query.</returns>
        public TResult Execute<TResult>(Expression expression)
        {
            return inner.Execute<TResult>(expression);
        }

        /// <summary>
        /// Executes the query asynchronously and returns an asynchronous enumerable result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="expression">The expression representing the query.</param>
        /// <returns>An <see cref="IAsyncEnumerable{T}"/> result of the query.</returns>
        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
        {
            return new TestAsyncEnumerable<TResult>(expression);
        }

        /// <summary>
        /// Executes the query asynchronously and returns a typed result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="expression">The expression representing the query.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>The typed result of the query.</returns>
        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Execute<TResult>(expression);
        }
    }
}
