using System;
using System.Web.Http.Dependencies;

namespace refactor_me.DependencyResolver
{
    /// <summary>
    /// Dependencyresolver interface used by unity
    /// </summary>
    public interface IDependencyResolver : IDependencyScope, IDisposable
    {
        /// <summary>
        /// Begins the scope
        /// </summary>
        /// <returns>The dependency scope used by dependency injection</returns>
        IDependencyScope BeginScope();
    }
}
