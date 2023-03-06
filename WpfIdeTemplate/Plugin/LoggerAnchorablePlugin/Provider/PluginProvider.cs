using System;
using Microsoft.Extensions.DependencyInjection;
using SampleCompany.SampleProduct.CommonLibrary.InMemoryLogger;
using SampleCompany.SampleProduct.LoggerAnchorablePlugin.ViewModel;
using SampleCompany.SampleProduct.PluginUtility;

namespace SampleCompany.SampleProduct.LoggerAnchorablePlugin.Provider
{
    public class PluginProvider : IPluginProvider
    {
        public object CreatePluginObject(IServiceProvider provider)
        {
            var store = provider.GetRequiredService<IInMemoryLogStore>();
            return new LoggerAnchorableViewModel(store);
        }
        public PluginProvider()
        {

        }
    }
}
