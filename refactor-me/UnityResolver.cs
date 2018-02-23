using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Unity;
using Unity.Exceptions;

namespace refactor_me
{
    /// <summary>
    /// The UnityResolver class used to resolve the dependency
    /// </summary>
    public class UnityResolver : IDependencyResolver
    {
        protected IUnityContainer container;

        /// <summary>
        /// The UnityResolver constructor used to initialize the unity container
        /// </summary>
        /// <param name="container">The unity container</param>
        public UnityResolver(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.container = container;
        }

        /// <summary>
        /// Creates the child container
        /// </summary>
        /// <returns>Returns the child container</returns>
        IDependencyScope IDependencyResolver.BeginScope()
        {
            var child = container.CreateChildContainer();
            return new UnityResolver(child);
        }

        /// <summary>
        /// Dispose the container instance
        /// </summary>
        public void Dispose()
        {
            container.Dispose();
        }

        /// <summary>
        /// Gets the instance of the serviceType specified
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns>Returns the specified serviceType instance</returns>
        public object GetService(Type serviceType)
        {
            try
            {
                return container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {

                return null;
            }
        }

        /// <summary>
        /// Gets the instances of the serviceType specified
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns>Returns the specified serviceType instances</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {

                return new List<object>();
            }
        }
    }
}
