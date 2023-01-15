using SampleCompany.SampleProduct.CommonLibrary;
using SampleCompany.SampleProduct.LoggerAnchorablePlugin.ViewModel;
using SampleCompany.SampleProduct.PluginUtility;

namespace SampleCompany.SampleProduct.LoggerAnchorablePlugin.Provider
{
    public class PluginProvider : IPluginProvider
    {
        public object CreatePluginObject(IAppServiceProvider provider)
        {
            var store = provider.GetRequiredService<IInMemoryLogStore>();
            return new LoggerAnchorableViewModel(store);
        }
        public PluginProvider()
        {

        }
    }
}
