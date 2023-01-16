using Microsoft.Extensions.Logging;
using SampleCompany.SampleProduct.CommonLibrary.MessageBroker;
using SampleCompany.SampleProduct.CommonLibrary.MessageBroker.MessageStructure;
using SampleCompany.SampleProduct.PluginUtility;
using SampleCompany.SampleProduct.SampleAnchorablePlugin.ViewModel;

namespace SampleCompany.SampleProduct.SampleAnchorablePlugin.Provider
{
    public class PluginProvider : IPluginProvider
    {

        public object CreatePluginObject(IAppServiceProvider provider)
        {
            var logger = provider.GetRequiredService<ILogger<SampleAnchorableViewModel>>();
            var pub = provider.GetRequiredService<IAsyncSubscriber<SampleMessage>>();
            return new SampleAnchorableViewModel(logger, pub);
        }

        public PluginProvider()
        {

        }
    }
}
