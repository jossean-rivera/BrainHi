using BrainHi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BrainHi.Data
{
    /// <summary>
    /// Application DB repository for the BrainHi application
    /// </summary>
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly BrainHiContext _context;

        public ApplicationRepository(BrainHiContext context) => _context = context;

        /// <inheritdoc />
        public IQueryable<TEntity> GetQueryBuilder<TEntity>() where TEntity : class, IBrainHiObject => _context.Set<TEntity>();

        /// <inheritdoc />
        public async Task<IEnumerable<TEntity>> GetEntitiesAsync<TEntity>() where TEntity : class, IBrainHiObject =>
            await _context.Set<TEntity>().ToArrayAsync();

        /// <inheritdoc />
        public async Task<IEnumerable<TEntity>> GetEntitiesAsync<TEntity>(Expression<Func<TEntity, bool>> selector) where TEntity : class, IBrainHiObject =>
            await _context.Set<TEntity>().Where(selector).ToArrayAsync();

        /// <inheritdoc />
        public async Task<TEntity> GetEntityAsync<TEntity>(params object[] keyValues) where TEntity : class, IBrainHiObject =>
            await _context.FindAsync<TEntity>(keyValues);

        /// <inheritdoc />
        public async Task<TEntity> GetEntityAsync<TEntity>(Expression<Func<TEntity, bool>> selector) where TEntity : class, IBrainHiObject =>
            await _context.Set<TEntity>().FirstOrDefaultAsync(selector);

        /// <inheritdoc />
        public async Task<TEntity> AddEntityAsync<TEntity>(TEntity entity) where TEntity : class, IBrainHiObject
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TEntity>> AddEntitiesAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IBrainHiObject
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        /// <inheritdoc />
        public async Task<TEntity> UpdateEntityAsync<TEntity>(TEntity entity) where TEntity : class, IBrainHiObject
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TEntity>> UpdateEntitiesAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IBrainHiObject
        {
            _context.Set<TEntity>().UpdateRange(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        /// <inheritdoc />
        public async Task<TEntity> RemoveEntityAsync<TEntity>(TEntity entity) where TEntity : class, IBrainHiObject
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TEntity>> RemoveEntitiesAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IBrainHiObject
        {
            _context.Set<TEntity>().RemoveRange(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        /// <inheritdoc />
        public void Dispose() => _context.Dispose();
    }
}
