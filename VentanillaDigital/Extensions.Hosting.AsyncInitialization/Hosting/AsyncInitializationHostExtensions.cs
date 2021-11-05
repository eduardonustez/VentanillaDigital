using Extensions.Hosting.AsyncInitialization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// Provides extension methods to perform async initialization of an application.
    /// </summary>
    public static class AsyncInitializationHostExtensions
    {
        /// <summary>
        /// InitAsync
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static Task InitAsync(this WebAssemblyHost host)
        {
            if (host == null)
                throw new ArgumentNullException(nameof(host));

            using (var scope = host.Services.CreateScope())
            {
                var rootInitializer = scope.ServiceProvider.GetService<RootInitializer?>();
                if (rootInitializer == null)
                {
                    throw new InvalidOperationException("The async initialization service isn't registered, register it by calling AddAsyncInitialization() on the service collection or by adding an async initializer.");
                }

                _ = rootInitializer.InitializeAsync();
            }

            return Task.FromResult(0);
        }

        //public static async Task InitAsync(this WebAssemblyHost host)
        //{
        //    if (host == null)
        //        throw new ArgumentNullException(nameof(host));

        //    using (var scope = host.Services.CreateScope())
        //    {
        //        var rootInitializer = scope.ServiceProvider.GetService<RootInitializer?>();
        //        if (rootInitializer == null)
        //        {
        //            throw new InvalidOperationException("The async initialization service isn't registered, register it by calling AddAsyncInitialization() on the service collection or by adding an async initializer.");
        //        }

        //        await rootInitializer.InitializeAsync();
        //    }
        //}
    }
}
