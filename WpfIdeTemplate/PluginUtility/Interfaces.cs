using System;

namespace SampleCompany.SampleProduct.PluginUtility
{
    public interface IPluginProvider
    {
        public object CreatePluginObject(IAppServiceProvider provider);
    }
    public interface IAppServiceProvider
    {
        T? GetService<T>();
        object? GetService(Type serviceType);
        T GetRequiredService<T>() where T : notnull;
        object GetRequiredService(Type serviceType);
    }
}
