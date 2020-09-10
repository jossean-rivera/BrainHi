using BrainHi.Models;
using MockQueryable.Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BrainHi.Data
{
    /// <summary>
    /// Repository of application objects in memory (instead of an external database, or file)
    /// </summary>
    public class MemoryApplicationRepository : IApplicationRepository
    {
        //  Private list of providers to keep in memory
        private readonly List<Provider> _providers = new List<Provider>();

        //  Private list of appointments to keep in memory
        private readonly List<Appointment> _appointments = new List<Appointment>();

        /// <summary>
        /// Gets the correct private list between <see cref="_providers"/> and <see cref="_appointments"/> depending on their Type
        /// </summary>
        /// <typeparam name="T">Type of the list to get</typeparam>
        /// <returns>Appropriate list for the type equal to <see cref="T"/></returns>
        private IList<T> GetListForType<T>() where T : class, IBrainHiObject
        {
            if (typeof(T) == typeof(Provider))
                return _providers as IList<T>;
            else
                return _appointments as IList<T>;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _providers?.Clear();
            _appointments?.Clear();
        }

        /// <inheritdoc />
        Task<IEnumerable<TEntity>> IApplicationRepository.AddEntitiesAsync<TEntity>(IEnumerable<TEntity> entities)
        {
            //  Get the collection for the entities
            IList<TEntity> list = GetListForType<TEntity>();

            //  Iterate entities
            foreach (TEntity entity in entities)
            {
                //  Calculate the ID of the entity
                int id = list.Any() ? list.Max(e => e.Id) + 1 : 1;

                //  Set the ID of the entity
                entity.Id = id;

                if (entity is IProviderDependent providerDependent)
                {
                    //  Search for the provider and add the reference to the navigation property
                    providerDependent.Provider = _providers.FirstOrDefault(provider => provider.ProviderId == providerDependent.ProviderId);

                    if (entity is Appointment)
                    {
                        //  Add the appointment to the collection of appointments in the provider
                        providerDependent.Provider?.Appointments?.Add(entity as Appointment);
                    }
                }

                //  Add the entity
                list.Add(entity);
            }

            return Task.FromResult(entities);
        }

        /// <inheritdoc />
        Task<TEntity> IApplicationRepository.AddEntityAsync<TEntity>(TEntity entity)
        {
            //  Get the collection for the entities
            IList<TEntity> list = GetListForType<TEntity>();

            //  Calculate the ID of the entity
            int id = list.Any() ? list.Max(e => e.Id) + 1 : 1;

            //  Set the ID of the entity
            entity.Id = id;

            if (entity is IProviderDependent providerDependent)
            {
                //  Search for the provider and add the reference to the navigation property
                providerDependent.Provider = _providers.FirstOrDefault(provider => provider.ProviderId == providerDependent.ProviderId);

                if (entity is Appointment)
                {
                    //  Add the appointment to the collection of appointments in the provider
                    providerDependent.Provider?.Appointments?.Add(entity as Appointment);
                }
            }

            //  Add the entity
            list.Add(entity);

            return Task.FromResult(entity);
        }

        /// <inheritdoc />
        Task<IEnumerable<TEntity>> IApplicationRepository.GetEntitiesAsync<TEntity>()
        {
            //  Get the collection for the entities
            IEnumerable<TEntity> list = GetListForType<TEntity>();

            //  Return list
            return Task.FromResult(list);
        }

        /// <inheritdoc />
        Task<IEnumerable<TEntity>> IApplicationRepository.GetEntitiesAsync<TEntity>(Expression<Func<TEntity, bool>> selector)
        {
            //  Get the collection for the entities
            IEnumerable<TEntity> list = GetListForType<TEntity>();

            //  Select the entity
            IEnumerable<TEntity> entities = list.Where(selector.Compile());

            //  Return entity
            return Task.FromResult(entities);
        }

        /// <inheritdoc />
        Task<TEntity> IApplicationRepository.GetEntityAsync<TEntity>(params object[] keyValues)
        {
            //  Get the collection for the entities
            IEnumerable<TEntity> list = GetListForType<TEntity>();

            //  Select the entity
            TEntity entity = list.FirstOrDefault(e => e.Id.Equals(keyValues.First()));

            //  Return entity
            return Task.FromResult(entity);
        }

        /// <inheritdoc />
        Task<TEntity> IApplicationRepository.GetEntityAsync<TEntity>(Expression<Func<TEntity, bool>> selector)
        {
            //  Get the collection for the entities
            IEnumerable<TEntity> list = GetListForType<TEntity>();

            //  Select the entity
            TEntity entity = list.FirstOrDefault(selector.Compile());

            //  Return entity
            return Task.FromResult(entity);
        }

        /// <inheritdoc />
        IQueryable<TEntity> IApplicationRepository.GetQueryBuilder<TEntity>()
        {
            //  Get the collection for the entities
            IList<TEntity> list = GetListForType<TEntity>();

            //  Return list
            return list.AsQueryable().BuildMockDbSet().Object;
        }

        /// <inheritdoc />
        Task<IEnumerable<TEntity>> IApplicationRepository.RemoveEntitiesAsync<TEntity>(IEnumerable<TEntity> entities)
        {
            //  Get the collection for the entities
            IList<TEntity> list = GetListForType<TEntity>();

            //  Iterate entities to remove
            foreach (TEntity entity in entities)
            {
                //  Find the entity in list
                TEntity entityToRemove = list.FirstOrDefault(e => e.Id == entity.Id);

                //  If we found it, then remove it
                if (entityToRemove != null)
                {
                    list.Remove(entityToRemove);
                }
            }

            return Task.FromResult(entities);
        }

        /// <inheritdoc />
        Task<TEntity> IApplicationRepository.RemoveEntityAsync<TEntity>(TEntity entity)
        {
            //  Get the collection for the entities
            IList<TEntity> list = GetListForType<TEntity>();

            //  Find the entity in list
            TEntity entityToRemove = list.FirstOrDefault(e => e.Id == entity.Id);

            //  If we found it, then remove it
            if (entityToRemove != null)
            {
                list.Remove(entityToRemove);
            }

            //  Return removed entitty
            return Task.FromResult(entity);
        }

        /// <inheritdoc />
        Task<IEnumerable<TEntity>> IApplicationRepository.UpdateEntitiesAsync<TEntity>(IEnumerable<TEntity> entities)
        {
            //  Get the collection for the entities
            IList<TEntity> list = GetListForType<TEntity>();

            //  Iterate entities to update
            foreach (TEntity entity in entities)
            {
                //  Find the entity in list
                TEntity entityToRemove = list.FirstOrDefault(e => e.Id == entity.Id);

                //  If we found it, then remove it and add the new entity
                if (entityToRemove != null)
                {
                    list.Remove(entityToRemove);
                    list.Add(entity);
                }
            }

            //  Return updated entities
            return Task.FromResult(entities);
        }

        /// <inheritdoc />
        Task<TEntity> IApplicationRepository.UpdateEntityAsync<TEntity>(TEntity entity)
        {
            //  Get the collection for the entities
            IList<TEntity> list = GetListForType<TEntity>();

            //  Find the entity in list
            TEntity entityToRemove = list.FirstOrDefault(e => e.Id == entity.Id);

            //  If we found it, then remove it and add the new entity
            if (entityToRemove != null)
            {
                list.Remove(entityToRemove);
                list.Add(entity);
            }

            //  Return update entity
            return Task.FromResult(entity);
        }
    }
}
