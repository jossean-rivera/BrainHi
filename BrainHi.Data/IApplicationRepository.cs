using BrainHi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BrainHi.Data
{
    /// <summary>
    /// Repository of all objects in the objects of the application
    /// </summary>
    /// <remarks>
    /// The purpose of the interface is to add an extra layer of abstraction for the <see cref="BrainHiContext"/> so that we could use 
    /// many different types of DB Context of any collection object. It makes Unit testing easier as well.
    /// </remarks>
    public interface IApplicationRepository : IDisposable
    {
        /// <summary>
        /// Gets object to build query
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <returns>Query of TEntity objects</returns>
        IQueryable<TEntity> GetQueryBuilder<TEntity>() where TEntity : class, IBrainHiObject;

        /// <summary>
        /// Gets entire set of entities with type of <see cref="TEntity"/> from the repository
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <returns>Enumeration of <see cref="TEntity"/> objects</returns>
        Task<IEnumerable<TEntity>> GetEntitiesAsync<TEntity>() where TEntity : class, IBrainHiObject;

        /// <summary>
        /// Gets subset of entities with type of <see cref="TEntity"/> from the repository
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="selector">Function to select which entities will be returned</param>
        /// <returns>Enumeration of <see cref="TEntity"/> objects</returns>
        Task<IEnumerable<TEntity>> GetEntitiesAsync<TEntity>(Expression<Func<TEntity, bool>> selector) where TEntity : class, IBrainHiObject;

        /// <summary>
        /// Returns a single instance of type of <see cref="TEntity"/>
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="keyValues">Values of the primary keys of the entity</param>
        /// <returns>Single instance of <see cref="TEntity"/></returns>
        Task<TEntity> GetEntityAsync<TEntity>(params object[] keyValues) where TEntity : class, IBrainHiObject;

        /// <summary>
        /// Returns a single instance of type of <see cref="TEntity"/>
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="selector">Expression to select what entity will be returned</param>
        /// <returns>Single instance of <see cref="TEntity"/></returns>
        Task<TEntity> GetEntityAsync<TEntity>(Expression<Func<TEntity, bool>> selector) where TEntity : class, IBrainHiObject;

        /// <summary>
        /// Adds new entity to the repository
        /// </summary>
        /// <remarks>
        /// If the values of the primary key properties are not set to its default value, this action will fail with the <see cref="BrainHiContext"/>
        /// </remarks>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="entity">Entity to add</param>
        /// <returns>New Added entity</returns>
        Task<TEntity> AddEntityAsync<TEntity>(TEntity entity) where TEntity : class, IBrainHiObject;

        /// <summary>
        /// Adds new entities to the repository
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="entities">Entities to add</param>
        /// <returns>New Added Entities</returns>
        Task<IEnumerable<TEntity>> AddEntitiesAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IBrainHiObject;

        /// <summary>
        /// Updates an existing entity at the repository
        /// </summary>
        /// <remarks>
        /// The values of the primary key properties must be populated with real data in order for the <see cref="BrainHiContext"/> to work
        /// </remarks>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="entity">Entity to update</param>
        /// <returns>Updated entity</returns>
        Task<TEntity> UpdateEntityAsync<TEntity>(TEntity entity) where TEntity : class, IBrainHiObject;

        /// <summary>
        /// Updates existing entities to the repository
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="entities">Entities to update</param>
        /// <returns>Updated Entities</returns>
        Task<IEnumerable<TEntity>> UpdateEntitiesAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IBrainHiObject;

        /// <summary>
        /// Removes an existing entity from the repository
        /// </summary>
        /// <remarks>
        /// The values of the primary key properties must be populated with real data in order for the <see cref="BrainHiContext"/> to work
        /// </remarks>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="entity">Entity to remove</param>
        /// <returns>Removed entity</returns>
        Task<TEntity> RemoveEntityAsync<TEntity>(TEntity entity) where TEntity : class, IBrainHiObject;

        /// <summary>
        /// Removes existing entities to the repository
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="entities">Entities to remove</param>
        /// <returns>Removed Entities</returns>
        Task<IEnumerable<TEntity>> RemoveEntitiesAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IBrainHiObject;
    }
}
