using Microsoft.Extensions.Logging;
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
            return new SampleDocumentViewModel(logger, pulisher);
        }

        public PluginProvider()
        {

        }
    }
}
