using System;

namespace SampleCompany.SampleProduct.PluginUtility
{
    public interface IPluginProvider
    {
        public object CreatePluginObject(IServiceProvider provider);
    }
}
