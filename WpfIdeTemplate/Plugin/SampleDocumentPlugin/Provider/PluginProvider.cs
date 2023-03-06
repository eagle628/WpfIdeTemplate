using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SampleCompany.SampleProduct.ApplicationEngine.Proto;
using SampleCompany.SampleProduct.CommonLibrary.MessageBroker;
using SampleCompany.SampleProduct.CommonLibrary.MessageBroker.MessageStructure;
using SampleCompany.SampleProduct.PluginUtility;
using SampleCompany.SampleProduct.SampleDocumentPlugin.ViewModel;

namespace SampleCompany.SampleProduct.SampleDocumentPlugin.Provider
{
    public class PluginProvider : IPluginProvider
    {

        public object CreatePluginObject(IServiceProvider provider)
        {
            var logger = provider.GetRequiredService<ILogger<SampleDocumentViewModel>>();
            var pulisher = provider.GetRequiredService<IAsyncPublisher<SampleMessage>>();
            var greeterClient = provider.GetRequiredService<Greeter.GreeterClient>();
            var managementClient = provider.GetRequiredService<ApplicationInstanceManagement.ApplicationInstanceManagementClient>();
            return new SampleDocumentViewModel(logger, pulisher, greeterClient, managementClient);
        }

        public PluginProvider()
        {

        }
    }
}
