using System.Linq.Expressions;

namespace SubscriptionTracker.Web.Repositories.Interfaces
{
    /// <summary>
    /// Interface for generic repository operations
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <returns>IQueryable of entities</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Gets entities by specified condition
        /// </summary>
        /// <param name="predicate">The condition to filter entities</param>
        /// <returns>IQueryable of filtered entities</returns>
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Creates a new entity
        /// </summary>
        /// <param name="entity">The entity to create</param>
        /// <returns>The created entity</returns>
        Task<T> CreateAsync(T entity);

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <returns>The updated entity</returns>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Deletes an entity
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task DeleteAsync(T entity);

        /// <summary>
        /// Saves changes to the database
        /// </summary>
        /// <returns>Task representing the asynchronous operation</returns>
        Task SaveAsync();
    }
}