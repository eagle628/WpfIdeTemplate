using Microsoft.Extensions.Logging;
using SampleCompany.SampleProduct.PluginUtility;
using SampleCompany.SampleProduct.SampleDocumentPlugin.ViewModel;

namespace SampleCompany.SampleProduct.SampleDocumentPlugin.Provider
{
    public class PluginProvider : IPluginProvider
    {

        public object CreatePluginObject(IAppServiceProvider provider)
        {
            var logger = provider.GetRequiredService<ILogger<SampleDocumentViewModel>>();
            return new SampleDocumentViewModel(logger);
        }

        public PluginProvider()
        {

        }
    }
}
