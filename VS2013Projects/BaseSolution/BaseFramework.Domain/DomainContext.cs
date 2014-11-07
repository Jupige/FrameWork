using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;

namespace BaseFramework.Domain
{
        /// <summary>
        /// <see cref="IDomainContext"/> defines an interface to mark an entity added to or removed from the context,
        /// or retrieve specified type entity set.
        /// 
        /// Especially attention:
        /// This interface should be derived when using in the concreate business domain.
        /// </summary>
        public interface IDomainContext
        {
            /// <summary>
            /// Check if the context has been relased.
            /// </summary>
            bool IsDisposed { get; set; }

            /// <summary>
            /// Explicitly mark the entity has been added in the context
            /// </summary>
            /// <typeparam name="TEntity">entity type</typeparam>
            /// <param name="entity">entity instance</param>
            TEntity Add<TEntity>(TEntity entity) where TEntity : class;

            /// <summary>
            /// Explicitly mark the entity has been modified in the context
            /// </summary>
            /// <typeparam name="TEntity"></typeparam>
            /// <param name="entity"></param>
            void Modify<TEntity>(TEntity entity) where TEntity : class;

            /// <summary>
            /// Explicitly mark the entity has been removed form the context
            /// </summary>
            /// <typeparam name="TEntity">entity type</typeparam>
            /// <param name="entity">entity instance</param>
            void Remove<TEntity>(TEntity entity) where TEntity : class;

            /// <summary>
            /// Get entity set
            /// </summary>
            /// <typeparam name="TEntity">entity type</typeparam>
            /// <returns>Entity set supporting LinQ</returns>
            IQueryable<TEntity> EntitySet<TEntity>() where TEntity : class;

            /// <summary>
            ///  Get entity set
            /// </summary>
            /// <typeparam name="TEntity">entity type</typeparam>
            /// <param name="predicate">predicate expression</param>
            /// <returns>Entity set supporting LinQ</returns>
            IQueryable<TEntity> EntitySet<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

            /// <summary>
            /// Find entity by key values
            /// </summary>
            /// <typeparam name="TEntity">entity type</typeparam>
            /// <param name="keyValues">key values</param>
            /// <returns>if exists return the entity instance, otherwise return null</returns>
            TEntity Find<TEntity>(params object[] keyValues) where TEntity : class;

            /// <summary>
            /// Submit entity changes
            /// </summary>
            void SubmitChanges();

            /// <summary>
            /// Commit transaction
            /// </summary>
            void CommitTransaction();

            /// <summary>
            /// Get memory store to put temporary objects in the current context;
            /// And one type has an isolated space.
            /// </summary>
            /// <typeparam name="T">object</typeparam>
            /// <returns></returns>
            List<T> MemoryStore<T>();

            /// <summary>
            /// Add object into the memory store;
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="obj"></param>
            void AddToMemory<T>(T obj);

            /// <summary>
            /// Remove object from the memory store
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="obj"></param>
            void RemoveFromMemory<T>(T obj);
        }

        /// <summary>
        /// Domain context with which the domain model can access entities
        /// </summary>
        public abstract class DomainContext : IDomainContext
        {
            private readonly Dictionary<Type, object> _memoryStore = new Dictionary<Type, object>();

            public bool IsDisposed { get; set; }

            public abstract TEntity Add<TEntity>(TEntity entity) where TEntity : class;

            public abstract void Modify<TEntity>(TEntity entity) where TEntity : class;

            public abstract void Remove<TEntity>(TEntity entity) where TEntity : class;

            public abstract IQueryable<TEntity> EntitySet<TEntity>() where TEntity : class;

            public abstract IQueryable<TEntity> EntitySet<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

            public abstract TEntity Find<TEntity>(params object[] keyValues) where TEntity : class;

            public abstract void SubmitChanges();

            public abstract void CommitTransaction();

            public virtual List<T> MemoryStore<T>()
            {
                if (!_memoryStore.ContainsKey(typeof(T)))
                    _memoryStore[typeof(T)] = new List<T>();

                return (List<T>)_memoryStore[typeof(T)];
            }

            public virtual void AddToMemory<T>(T obj)
            {
                MemoryStore<T>().Add(obj);
            }

            public virtual void RemoveFromMemory<T>(T obj)
            {
                var store = MemoryStore<T>();

                if (store.Contains(obj))
                    store.Remove(obj);
            }

            #region static methods

            /// <summary>
            /// Get the default domain context, once the domain contexts are more than one, please use GetContext method.
            /// If the current domain context does not exist, throw exception.
            /// </summary>
            public static IDomainContext Current
            {
                get
                {
                    return DomainManager.GetDefaultContext();
                }
            }

            /// <summary>
            /// Get the domain context with specified domain context interface type. 
            /// </summary>
            /// <typeparam name="DomainContextInterface">domain context interface</typeparam>
            /// <returns>if exists return domain context instance, otherwise return null</returns>
            public static DomainContextInterface GetContext<DomainContextInterface>()
                where DomainContextInterface : IDomainContext
            {
                return DomainManager.GetContext<DomainContextInterface>();
            }

            #endregion

        }

        /// <summary>
        /// <see cref="DomainManager"/> is responsible for manage domain contexts and services that will be consumed by domain model. 
        /// </summary>
        public class DomainManager
        {
            static readonly Dictionary<Type, object> GlobalServices = new Dictionary<Type, object>();

            /// <summary>
            /// Domain contexts live in the call context
            /// </summary>
            static Dictionary<Type, object> DomainContexts
            {
                get
                {
                    var domainContexts = CallContext.GetData("__DomainContexts") as Dictionary<Type, object>;

                    if (domainContexts == null)
                    {
                        domainContexts = new Dictionary<Type, object>();
                        CallContext.SetData("__DomainContexts", domainContexts);
                    }

                    return domainContexts;
                }
            }

            /// <summary>
            /// Register domain context
            /// </summary>
            /// <typeparam name="DomainContextInterface">domain context interface</typeparam>
            /// <param name="domainContext">domain context instance</param>
            public static void RegisterContext<DomainContextInterface>(DomainContextInterface domainContext)
                where DomainContextInterface : IDomainContext
            {
                if (DomainContexts.ContainsKey(typeof(DomainContextInterface))
                    && false == ((DomainContextInterface)DomainContexts[typeof(DomainContextInterface)]).IsDisposed)
                    throw new ApplicationException(string.Format("DomainContext conflict. There has existed an instance of {0} whose 'IsDiposed' is false. Please make sure close current domain context before open another.", typeof(DomainContextInterface).FullName));

                DomainContexts[typeof(DomainContextInterface)] = domainContext;
            }

            /// <summary>
            /// Unregister domain context
            /// </summary>
            /// <typeparam name="DomainContextInterface">domain context interface</typeparam>
            public static void UnregisterContext<DomainContextInterface>()
                where DomainContextInterface : IDomainContext
            {
                if (DomainContexts.ContainsKey(typeof(DomainContextInterface)))
                    DomainContexts.Remove(typeof(DomainContextInterface));
            }

            /// <summary>
            /// Get domain context
            /// </summary>
            /// <typeparam name="DomainContextInterface">domain context interface</typeparam>
            /// <returns>domain context instance</returns>
            public static DomainContextInterface GetContext<DomainContextInterface>()
                where DomainContextInterface : IDomainContext
            {
                return (DomainContextInterface)DomainContexts[typeof(DomainContextInterface)];
            }

            /// <summary>
            /// Get default domain context
            /// </summary>
            /// <returns>default domain context instance</returns>
            public static IDomainContext GetDefaultContext()
            {
                if (DomainContexts.Count >= 0)
                    return (IDomainContext)DomainContexts.FirstOrDefault().Value;
                else
                    throw new ApplicationException("Cannot find any registered DomainContext!");
            }

            /// <summary>
            /// Register service implementation with the service interface in the current call context
            /// </summary>
            /// <typeparam name="IService">service interface</typeparam>
            /// <param name="service">service instance</param>
            public static void RegisterContextService<IService>(IService service)
            {
                CallContext.SetData("__DomainContext_" + typeof(IService).FullName, service);
            }

            /// <summary>
            /// Get service instance with the service interface in the current call context
            /// </summary>
            /// <typeparam name="IService">service interface</typeparam>
            /// <returns>if exists return service instance,otherwise return null</returns>
            public static IService GetContextService<IService>()
            {
                return (IService)CallContext.GetData("__DomainContext_" + typeof(IService).FullName);
            }

            /// <summary>
            /// Register service implementation with the service interface 
            /// </summary>
            /// <typeparam name="IService">service interface</typeparam>
            /// <param name="service">service instance</param>
            public static void RegisterGlobalService<IService>(IService service)
            {
                GlobalServices[typeof(IService)] = service;
            }

            /// <summary>
            /// Get service instance with the service interface
            /// </summary>
            /// <typeparam name="IService">service interface</typeparam>
            /// <returns>if exists return service instance,otherwise return null</returns>
            public static IService GetGlobalService<IService>()
            {
                return (IService)GlobalServices[typeof(IService)];
            }
        }
}
