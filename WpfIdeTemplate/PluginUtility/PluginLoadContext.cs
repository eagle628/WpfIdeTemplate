using System;
using System.Reflection;
using System.Runtime.Loader;

namespace SampleCompany.SampleProduct.PluginUtility
{
    /// <summary>
    /// AssemblyLoad with DependencyResolver
    /// </summary>
    /// <remarks>
    /// <see href="https://learn.microsoft.com/ja-jp/dotnet/core/tutorials/creating-app-with-plugin-support"/>
    /// <see cref="AssemblyDependencyResolver"/> be using, you add 
    /// PropertyGroup : EnableDynamicLoading Element true
    /// ProjectRefrece : Private Element set false, ExcludeAssets Element set runtime (Only Dynamic share project)
    /// in plugin project file (.csporj).
    /// </remarks>
    public class PluginLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver _resolver;

        public PluginLoadContext(string pluginPath)
        {
            _resolver = new AssemblyDependencyResolver(pluginPath);
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            string? assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath is not null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string? libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath is not null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }

            return IntPtr.Zero;
        }
    }
}
