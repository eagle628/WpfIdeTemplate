using SampleCompany.SampleProduct.PluginUtility;
using SampleCompany.SampleProduct.SampleDocumentPlugin.ViewModel;

namespace SampleCompany.SampleProduct.SampleDocumentPlugin.Provider
{
    public class PluginProvider : IPluginProvider
    {

        public object CreatePluginObject(IAppServiceProvider provider)
        {
            return new SampleDocumentViewModel();
        }

        public PluginProvider()
        {

        }
    }
}
