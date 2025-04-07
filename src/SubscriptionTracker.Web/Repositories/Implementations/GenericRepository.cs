using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SubscriptionTracker.Web.Data;
using SubscriptionTracker.Web.Repositories.Interfaces;

namespace SubscriptionTracker.Web.Repositories.Implementations
{
    /// <summary>
    /// Generic repository implementation for common database operations
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        /// <summary>
        /// The database context
        /// </summary>
        protected readonly ApplicationDbContext _context;

        /// <summary>
        /// The entity set
        /// </summary>
        protected readonly DbSet<T> _dbSet;

        /// <summary>
        /// Initializes a new instance of the GenericRepository class
        /// </summary>
        /// <param name="context">The database context</param>
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        /// <inheritdoc/>
        public virtual IQueryable<T> GetAll()
        {
            return _dbSet.AsNoTracking();
        }

        /// <inheritdoc/>
        public virtual IQueryable<T> GetByCondition(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).AsNoTracking();
        }

        /// <inheritdoc/>
        public virtual async Task<T> CreateAsync(T entity)
        {
            var result = await _dbSet.AddAsync(entity);
            return result.Entity;
        }

        /// <inheritdoc/>
        public virtual async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return await Task.FromResult(entity);
        }

        /// <inheritdoc/>
        public virtual async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }

        /// <inheritdoc/>
        public virtual async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}