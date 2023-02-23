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

        public object CreatePluginObject(IAppServiceProvider provider)
        {
            var logger = provider.GetRequiredService<ILogger<SampleDocumentViewModel>>();
            var pulisher = provider.GetRequiredService<IAsyncPublisher<SampleMessage>>();
            var client = provider.GetRequiredService<Greeter.GreeterClient>();
            return new SampleDocumentViewModel(logger, pulisher, client);
        }

        public PluginProvider()
        {

        }
    }
}
