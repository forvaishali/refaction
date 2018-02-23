using System;
using System.Collections.Generic;

namespace refactor_me.DependencyResolver
{
    /// <summary>
    /// IDependencyScope interface used by unity to create instance of the specified serviceType
    /// </summary>
    public interface IDependencyScope : IDisposable
    {
        /// <summary>
        /// Gets the instance of the serviceType specified
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns>Returns the specified serviceType instance</returns>
        object GetService(Type serviceType);

        /// <summary>
        /// Gets the instances of the serviceType specified
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns>Returns the specified serviceType instances</returns>
        IEnumerable<object> GetServices(Type serviceType);
    }
}
