using SampleCompany.SampleProduct.PluginUtility;
using SampleCompany.SampleProduct.SampleAnchorablePlugin.ViewModel;

namespace SampleCompany.SampleProduct.SampleAnchorablePlugin.Provider
{
    public class PluginProvider : IPluginProvider
    {

        public object CreatePluginObject(IAppServiceProvider provider)
        {
            return new SampleAnchorableViewModel();
        }

        public PluginProvider()
        {

        }
    }
}
