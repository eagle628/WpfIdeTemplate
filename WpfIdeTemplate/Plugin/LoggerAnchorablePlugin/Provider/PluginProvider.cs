using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SampleCompany.SampleProduct.ApplicationEngine.Proto;
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
            var client = provider.GetRequiredService<ApplicationIEngineLogging.ApplicationIEngineLoggingClient>();
            return new LoggerAnchorableViewModel(store, client);
        }
        public PluginProvider()
        {

        }
    }
}
